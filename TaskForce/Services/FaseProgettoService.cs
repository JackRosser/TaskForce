using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Enum;
using TaskForce.Models;

namespace TaskForce.Services;

public class FaseProgettoService(AppDbContext db) : IFaseProgettoService
{
    public async Task<GetFaseDto> CreateAsync(int macroFaseId, CreateFaseDto dto, CancellationToken ct = default)
    {
        var entity = new FaseProgetto
        {
            MacroFaseId = macroFaseId,
            Nome = dto.Nome!,
            GiorniPrevistiBe = dto.GiorniPrevistiBe,
            GiorniPrevistiUi = dto.GiorniPrevistiUi,
            Stato = StatoFase.Dacompletare
        };

        db.FasiProgetto.Add(entity);
        await db.SaveChangesAsync(ct);

        return new GetFaseDto
        {
            Id = entity.Id,
            MacroFaseId = entity.MacroFaseId,
            Nome = entity.Nome,
            GiorniPrevistiBe = entity.GiorniPrevistiBe ?? 0,
            GiorniPrevistiUi = entity.GiorniPrevistiUi ?? 0,
            Stato = entity.Stato
        };
    }

    public Task<GetFaseDto?> GetByIdAsync(int macroFaseId, int id, CancellationToken ct = default) =>
        db.FasiProgetto
          .AsNoTracking()
          .Where(f => f.MacroFaseId == macroFaseId && f.Id == id)
          .Select(f => new GetFaseDto
          {
              Id = f.Id,
              MacroFaseId = f.MacroFaseId,
              Nome = f.Nome,
              GiorniPrevistiBe = f.GiorniPrevistiBe ?? 0,
              GiorniPrevistiUi = f.GiorniPrevistiUi ?? 0,
              Stato = f.Stato
          })
          .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<GetFaseDto>> GetAllAsync(int macroFaseId, CancellationToken ct = default)
    {
        var list = await db.FasiProgetto
            .AsNoTracking()
            .Where(f => f.MacroFaseId == macroFaseId)
            .OrderBy(f => f.Id)
            .Select(f => new GetFaseDto
            {
                Id = f.Id,
                MacroFaseId = f.MacroFaseId,
                Nome = f.Nome,
                GiorniPrevistiBe = f.GiorniPrevistiBe ?? 0,
                GiorniPrevistiUi = f.GiorniPrevistiUi ?? 0,
                Stato = f.Stato
            })
            .ToListAsync(ct);

        return list; // IEnumerable<GetFaseDto>
    }

    public async Task<bool> UpdateAsync(UpdateFaseProgettoDto dto, CancellationToken ct = default)
    {
        var affected = await db.FasiProgetto
            .Where(f => f.Id == dto.Id && f.MacroFaseId == dto.MacroFaseId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(f => f.Nome, dto.Nome)
                .SetProperty(f => f.GiorniPrevistiBe, dto.GiorniPrevistiBe)
                .SetProperty(f => f.GiorniPrevistiUi, dto.GiorniPrevistiUi)
                .SetProperty(f => f.Stato, dto.Stato),
                ct);

        return affected == 1;
    }

    public async Task<bool> DeleteAsync(int macroFaseId, int id, CancellationToken ct = default)
    {
        var affected = await db.FasiProgetto
            .Where(f => f.MacroFaseId == macroFaseId && f.Id == id)
            .ExecuteDeleteAsync(ct);

        return affected == 1;
    }

}
