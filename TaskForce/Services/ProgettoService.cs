using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
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
                .Select(m => new GetMacroFaseDettaglioDto
                {
                    Id = m.Id,
                    Nome = m.Nome,
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











}
