using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
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

    public async Task UpdateAsync(int id, UpdateMacroFaseDto dto, CancellationToken ct)
    {
        var macroFase = await db.MacroFasi.FindAsync([id], ct);
        if (macroFase is null)
            throw new KeyNotFoundException($"MacroFase con id {id} non trovata");

        macroFase.Nome = dto.Nome!;
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var macroFase = await db.MacroFasi.FindAsync([id], ct);
        if (macroFase is null)
            throw new KeyNotFoundException($"MacroFase con id {id} non trovata");

        db.MacroFasi.Remove(macroFase);
        await db.SaveChangesAsync(ct);
    }

}
