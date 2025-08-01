using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;
using TaskForce.Models;

namespace TaskForce.Services;

public class MacroFaseService(AppDbContext db) : IMacroFaseService
{
    public async Task<GetMacroFaseDto> CreateAsync(int progettoId, CreateMacroFaseDto dto, CancellationToken ct = default)
    {
        var entity = new MacroFase { ProgettoId = progettoId, Nome = dto.Nome! };
        db.MacroFasi.Add(entity);
        await db.SaveChangesAsync(ct);

        return new GetMacroFaseDto { Id = entity.Id, ProgettoId = entity.ProgettoId, Nome = entity.Nome };
    }

    public Task<GetMacroFaseDto?> GetByIdAsync(int progettoId, int id, CancellationToken ct = default) =>
        db.MacroFasi
          .AsNoTracking()
          .Where(m => m.ProgettoId == progettoId && m.Id == id)
          .Select(m => new GetMacroFaseDto { Id = m.Id, ProgettoId = m.ProgettoId, Nome = m.Nome })
          .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<GetMacroFaseDto>> GetAllAsync(int progettoId, CancellationToken ct = default)
    {
        var list = await db.MacroFasi
            .AsNoTracking()
            .Where(m => m.ProgettoId == progettoId)
            .OrderBy(m => m.Id)
            .Select(m => new GetMacroFaseDto { Id = m.Id, ProgettoId = m.ProgettoId, Nome = m.Nome })
            .ToListAsync(ct);

        return list;
    }

    public async Task<bool> UpdateAsync(UpdateMacroFaseDto dto, CancellationToken ct = default)
    {
        var affected = await db.MacroFasi
            .Where(m => m.Id == dto.Id && m.ProgettoId == dto.ProgettoId)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.Nome, dto.Nome!), ct);

        return affected == 1;
    }

    public async Task<bool> DeleteAsync(int progettoId, int id, CancellationToken ct = default)
    {
        var affected = await db.MacroFasi
            .Where(m => m.ProgettoId == progettoId && m.Id == id)
            .ExecuteDeleteAsync(ct);

        return affected == 1;
    }

    public async Task<IEnumerable<MacroFaseWithFasiDto>> GetAllWithFasiAsync(int progettoId, CancellationToken ct = default)
    {
        var data = await db.MacroFasi
            .AsNoTracking()
            .Where(m => m.ProgettoId == progettoId)
            .OrderBy(m => m.Id)
            .Select(m => new MacroFaseWithFasiDto
            {
                Id = m.Id,
                ProgettoId = m.ProgettoId,
                Nome = m.Nome,
                Fasi = db.FasiProgetto
                    .Where(f => f.MacroFaseId == m.Id)
                    .OrderBy(f => f.Id)
                    .Select(f => new GetFaseDto
                    {
                        Id = f.Id,
                        MacroFaseId = f.MacroFaseId,
                        Nome = f.Nome,
                        GiorniPrevisti = f.GiorniPrevisti ?? 0,
                        Stato = f.Stato
                    })
            })
            .ToListAsync(ct);

        return data;
    }
}
