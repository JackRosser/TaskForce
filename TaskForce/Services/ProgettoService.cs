using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto;
using TaskForce.Models;

namespace TaskForce.Services;

public class ProgettoService(AppDbContext db) : IProgettoService
{
    public async Task<GetProgettoDto> CreateAsync(CreaProgettoDto dto, CancellationToken ct = default)
    {
        var entity = new Progetto { Nome = dto.Nome!, Consegna = dto.Consegna };
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

    private double CalcolaTempoLavorato(PresaInCarico presa)
    {
        if (presa.DataFineLavoro is null)
            return 0;

        var durata = presa.DataFineLavoro.Value - presa.DataPresaInCarico;
        return durata.TotalSeconds;
    }


    public async Task<IEnumerable<GetProgettoWithFasiRequest>> GetAllWithInfoAsync(CancellationToken ct = default)
    {
        var progetti = await db.Progetti
            .AsNoTracking()
            .ToListAsync(ct);

        var macroFasi = await db.MacroFasi
            .AsNoTracking()
            .ToListAsync(ct);

        var fasi = await db.FasiProgetto
            .AsNoTracking()
            .ToListAsync(ct);

        var prese = await db.PreseInCarico
            .AsNoTracking()
            .ToListAsync(ct);

        var utenti = await db.Users
            .AsNoTracking()
            .ToListAsync(ct);

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
                                    var secondiLavorati = CalcolaTempoLavorato(presa);
                                    var oreTotali = Math.Ceiling(secondiLavorati / 3600.0);
                                    var giorni = (int)(oreTotali / 8);
                                    var oreExtra = (int)(oreTotali % 8);
                                    if (oreTotali > 0 && oreExtra == 0) oreExtra = 1;

                                    return new GetPresaInCaricoDettaglioDto
                                    {
                                        Id = presa.Id,
                                        UserId = presa.UserId,
                                        UserName = user?.Nome ?? "N/D",
                                        Stato = presa.Stato,
                                        DataInizio = presa.DataPresaInCarico,
                                        DataFine = presa.DataFineLavoro,
                                        GiorniEffettivi = giorni,
                                        OreEffettiveExtra = oreExtra
                                    };
                                })
                            };
                        })
                })
        });

        return result;
    }
}
