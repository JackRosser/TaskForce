using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto;
using TaskForce.Dto.Progetto;
using TaskForce.Enum;
using TaskForce.Models;

namespace TaskForce.Services;

public class ProgettoService(AppDbContext db) : IProgettoService
{
    public async Task<GetProgettoDto> CreateAsync(CreaProgettoDto dto, CancellationToken ct = default)
    {
        var entity = new Progetto { Nome = dto.Nome!, Consegna = dto.Consegna.Value };
        db.Progetti.Add(entity);
        await db.SaveChangesAsync(ct);

        return new GetProgettoDto { Id = entity.Id, Nome = entity.Nome, Consegna = entity.Consegna };
    }

    public Task<GetProgettoDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Progetti
          .AsNoTracking()
          .Where(p => p.Id == id)
          .Select(p => new GetProgettoDto { Id = p.Id, Nome = p.Nome, Consegna = p.Consegna })
          .FirstOrDefaultAsync(ct);

    public Task<IEnumerable<GetProgettoDto>> GetAllAsync(CancellationToken ct = default) =>
        db.Progetti
          .AsNoTracking()
          .OrderBy(p => p.Id)
          .Select(p => new GetProgettoDto { Id = p.Id, Nome = p.Nome, Consegna = p.Consegna })
          .ToListAsync(ct)
          .ContinueWith(t => (IEnumerable<GetProgettoDto>)t.Result!, ct);

    public async Task<bool> UpdateAsync(UpdateProgettoDto dto, CancellationToken ct = default)
    {
        var affected = await db.Progetti
            .Where(p => p.Id == dto.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Nome, dto.Nome!)
                .SetProperty(p => p.Consegna, dto.Consegna),
                ct);

        return affected == 1;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var affected = await db.Progetti
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(ct);

        return affected == 1;
    }


    public async Task<IEnumerable<GetProgettoWithFasiRequest>> GetAllWithInfoAsync(CancellationToken ct = default)
    {
        var progetti = await db.Progetti.AsNoTracking().ToListAsync(ct);
        var macroFasi = await db.MacroFasi.AsNoTracking().ToListAsync(ct);
        var fasi = await db.FasiProgetto.AsNoTracking().ToListAsync(ct);
        var prese = await db.PreseInCarico.AsNoTracking().ToListAsync(ct);
        var utenti = await db.Users.AsNoTracking().ToListAsync(ct);
        var pause = await db.Pause.AsNoTracking().Where(p => p.DataFine != null).ToListAsync(ct);

        var result = progetti.Select(p => new GetProgettoWithFasiRequest
        {
            Id = p.Id,
            Nome = p.Nome,
            Consegna = p.Consegna,
            MacroFasi = macroFasi
                .Where(m => m.ProgettoId == p.Id)
                .OrderBy(m => m.Ordine)
                .Select(m => new GetMacroFaseDettaglioDto

                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Ordine = m.Ordine,
                    Fasi = fasi
                        .Where(f => f.MacroFaseId == m.Id)
                        .Select(f =>
                        {
                            var fasePrese = prese.Where(pi => pi.FaseProgettoId == f.Id);

                            return new GetFaseDettaglioDto
                            {
                                Id = f.Id,
                                Nome = f.Nome,
                                GiorniPrevisti = f.GiorniPrevisti ?? 0,
                                Stato = f.Stato,
                                TipoFase = f.TipoFase,
                                PreseInCarico = fasePrese.Select(presa =>
                                {
                                    var user = utenti.FirstOrDefault(u => u.Id == presa.UserId);

                                    int giorniEffettivi = 0;
                                    if (presa.Stato == StatoUtente.Concluso && presa.DataFineLavoro.HasValue)
                                    {
                                        var pauseUtente = pause
                                            .Where(p => p.PresaInCaricoId == presa.Id)
                                            .Select(p => (p.DataInizio, p.DataFine!.Value))
                                            .ToList();

                                        giorniEffettivi = CalcolaGiorniLavorati(
                                            presa.DataPresaInCarico,
                                            presa.DataFineLavoro.Value,
                                            pauseUtente);
                                    }


                                    return new GetPresaInCaricoDettaglioDto
                                    {
                                        Id = presa.Id,
                                        UserId = presa.UserId,
                                        UserName = user?.Nome ?? "N/D",
                                        Stato = presa.Stato,
                                        DataInizio = presa.DataPresaInCarico,
                                        DataFine = presa.DataFineLavoro,
                                        GiorniEffettivi = giorniEffettivi,
                                        GiorniPrevisti = f.GiorniPrevisti ?? 0
                                    };


                                })
                            };
                        })
                })
        });

        return result;
    }

    private static int CalcolaGiorniLavorati(
    DateTime inizio, DateTime fine,
    List<(DateTime inizio, DateTime fine)> pause)
    {
        const int OrePerGiorno = 8;

        // Calcolo ore effettive
        var oreTotali = (fine - inizio).TotalHours;

        // Somma pause
        var orePausa = pause
            .Sum(p =>
            {
                var start = p.inizio < inizio ? inizio : p.inizio;
                var end = p.fine > fine ? fine : p.fine;
                return end > start ? (end - start).TotalHours : 0;
            });

        var oreEffettive = oreTotali - orePausa;

        // 🛑 Limite massimo: 8 ore al giorno, per ogni giorno tra inizio e fine
        int giorniSolari = (int)Math.Ceiling((fine.Date - inizio.Date).TotalDays);
        if (giorniSolari == 0)
            giorniSolari = 1;

        double oreMassimeConsentite = giorniSolari * OrePerGiorno;

        oreEffettive = Math.Min(oreEffettive, oreMassimeConsentite);

        return oreEffettive <= 0 ? 0 : (int)Math.Ceiling(oreEffettive / OrePerGiorno);
    }



    //////////////////////////////////////////////////////////////////////
    ///Calcolo globale
    ///

    public async Task<IEnumerable<PortfolioProjectOverviewDto>> GetPortfolioOverviewAsync(CancellationToken ct = default)
    {
        var today = DateTime.Today;

        var progetti = await db.Progetti.AsNoTracking().ToListAsync(ct);
        var macroFasi = await db.MacroFasi.AsNoTracking().ToListAsync(ct);
        var fasi = await db.FasiProgetto.AsNoTracking().ToListAsync(ct);
        var prese = await db.PreseInCarico.AsNoTracking().ToListAsync(ct);
        var pause = await db.Pause.AsNoTracking().ToListAsync(ct);

        // Mappa: MacroFaseId -> ProgettoId
        var macroToProj = macroFasi.ToDictionary(m => m.Id, m => m.ProgettoId);

        var preliminari = progetti
            .Select(p =>
            {
                var fasiProgetto = fasi.Where(f => macroToProj.TryGetValue(f.MacroFaseId, out var pid) && pid == p.Id).ToList();

                // 🔴 SE QUI RISULTA 0, è perché nei dati GiorniPrevisti delle fasi è null/0
                var giorniInseriti = fasiProgetto.Sum(f => f.GiorniPrevisti ?? 0);

                var preseDelProgetto = prese.Where(pi => fasiProgetto.Any(f => f.Id == pi.FaseProgettoId)).ToList();

                var giorniLavorati = 0;
                foreach (var pi in preseDelProgetto)
                {
                    var inizio = pi.DataPresaInCarico;
                    var fine = pi.DataFineLavoro ?? DateTime.Now;
                    var pausePi = pause.Where(pp => pp.PresaInCaricoId == pi.Id)
                                       .Select(pp => (pp.DataInizio, (pp.DataFine ?? DateTime.Now)))
                                       .Select(t => (t.DataInizio, t.Item2))
                                       .ToList();
                    giorniLavorati += CalcolaGiorniGlobaliLavorati(inizio, fine, pausePi);
                }

                var rimanenti = Math.Max(0, giorniInseriti - giorniLavorati);

                return new
                {
                    Progetto = p,
                    GiorniInseriti = giorniInseriti,
                    GiorniLavorati = giorniLavorati,
                    GiorniRimanenti = rimanenti
                };
            })
            .Where(x => x.Progetto.Consegna.Date >= today && x.GiorniRimanenti > 0)
            .OrderBy(x => x.Progetto.Consegna)
            .ToList();

        var cumulatoCarico = 0;
        var output = new List<PortfolioProjectOverviewDto>();

        foreach (var x in preliminari)
        {
            var tot = WorkingDaysBetween(today, x.Progetto.Consegna);
            var netti = Math.Max(0, tot - cumulatoCarico);
            var slack = netti - x.GiorniRimanenti;

            var esito = slack switch
            {
                < 0 => TipoEsito.NonFattibile,
                0 => TipoEsito.Warning,
                _ => TipoEsito.Ok
            };

            output.Add(new PortfolioProjectOverviewDto
            {
                ProgettoId = x.Progetto.Id,
                Nome = x.Progetto.Nome,
                Consegna = x.Progetto.Consegna,
                GiorniInseriti = x.GiorniInseriti,
                GiorniLavorati = x.GiorniLavorati,
                GiorniRimanenti = x.GiorniRimanenti,
                GiorniLavorativiDisponibiliTot = tot,
                GiorniLavorativiDisponibiliNetti = netti,
                SlackGiorni = slack,
                Esito = esito
            });

            cumulatoCarico += x.GiorniRimanenti; // prenoto il carico
        }

        // --- progetti già completi o non rilevanti per il greedy (rimanenti == 0 o scadenza passata)
        var completatiONonRilevanti = progetti
            .Where(p => !preliminari.Any(y => y.Progetto.Id == p.Id))
            .Select(p =>
            {
                var fasiProgetto = fasi
                    .Where(f => macroToProj.TryGetValue(f.MacroFaseId, out var pid) && pid == p.Id)
                    .ToList();

                var giorniInseriti = fasiProgetto.Sum(f => f.GiorniPrevisti ?? 0);

                var preseDelProgetto = prese.Where(pi => fasiProgetto.Any(f => f.Id == pi.FaseProgettoId)).ToList();

                var giorniLav = 0;
                foreach (var pi in preseDelProgetto)
                {
                    var inizio = pi.DataPresaInCarico;
                    var fine = pi.DataFineLavoro ?? DateTime.Now;
                    var pausePi = pause.Where(pp => pp.PresaInCaricoId == pi.Id)
                                       .Select(pp => (pp.DataInizio, (pp.DataFine ?? DateTime.Now)))
                                       .Select(t => (t.DataInizio, t.Item2))
                                       .ToList();
                    giorniLav += CalcolaGiorniGlobaliLavorati(inizio, fine, pausePi);
                }

                var rimanenti = Math.Max(0, giorniInseriti - giorniLav);
                var tot = p.Consegna.Date >= DateTime.Today ? WorkingDaysBetween(DateTime.Today, p.Consegna) : 0;
                var netti = Math.Max(0, tot - /* prenotazione cumulata dei più urgenti */ 0); // fuori dal greedy

                return new PortfolioProjectOverviewDto
                {
                    ProgettoId = p.Id,
                    Nome = p.Nome,
                    Consegna = p.Consegna,
                    GiorniInseriti = giorniInseriti,
                    GiorniLavorati = giorniLav,
                    GiorniRimanenti = rimanenti,
                    GiorniLavorativiDisponibiliTot = tot,
                    GiorniLavorativiDisponibiliNetti = netti,
                    SlackGiorni = netti - rimanenti,
                    Esito = rimanenti == 0 ? TipoEsito.Ok
                          : (p.Consegna.Date < DateTime.Today ? TipoEsito.NonFattibile : TipoEsito.Warning)
                };
            });

        // ✅ QUI PASSA L’ENUMERABLE A CONCAT
        return output
            .Concat(completatiONonRilevanti)
            .OrderBy(o => o.Consegna)
            .ToList();

    }

    // Giorni lavorativi tra due date (inclusivo della scadenza, escluso il passato)
    private static int WorkingDaysBetween(DateTime from, DateTime to)
    {
        if (to.Date < from.Date) return 0;

        var d1 = from.Date;
        var d2 = to.Date;

        int days = 0;
        for (var d = d1; d <= d2; d = d.AddDays(1))
        {
            if (d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
                days++;
        }
        return days;
    }

    // (Già presente) — riuso con fine=Now se non conclusa
    private static int CalcolaGiorniGlobaliLavorati(
        DateTime inizio, DateTime fine,
        List<(DateTime inizio, DateTime fine)> pause)
    {
        const int OrePerGiorno = 8;

        var oreTotali = (fine - inizio).TotalHours;

        var orePausa = pause.Sum(p =>
        {
            var start = p.inizio < inizio ? inizio : p.inizio;
            var end = p.fine > fine ? fine : p.fine;
            return end > start ? (end - start).TotalHours : 0;
        });

        var oreEffettive = Math.Max(0, oreTotali - orePausa);

        // Limite 8h per ciascun giorno solare coinvolto
        int giorniSolari = (int)Math.Ceiling((fine.Date - inizio.Date).TotalDays);
        if (giorniSolari == 0) giorniSolari = 1;

        double oreMassimeConsentite = giorniSolari * OrePerGiorno;
        oreEffettive = Math.Min(oreEffettive, oreMassimeConsentite);

        return oreEffettive <= 0 ? 0 : (int)Math.Ceiling(oreEffettive / OrePerGiorno);
    }


}
