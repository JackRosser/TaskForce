using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Enum;
using TaskForce.Models;

public class FaseProgettoService(AppDbContext db) : IFaseProgettoService
{
    public async Task<GetFaseDto> CreateAsync(int macroFaseId, CreateFaseDto dto, CancellationToken ct = default)
    {
        var entity = new FaseProgetto
        {
            MacroFaseId = macroFaseId,
            Nome = dto.Nome!,
            GiorniPrevisti = dto.GiorniPrevisti,
            TipoFase = dto.TipoFase!.Value,
            Stato = StatoFase.DaCompletare
        };

        db.FasiProgetto.Add(entity);
        await db.SaveChangesAsync(ct);

        return new GetFaseDto
        {
            Id = entity.Id,
            MacroFaseId = entity.MacroFaseId,
            Nome = entity.Nome,
            GiorniPrevisti = entity.GiorniPrevisti ?? 0,
            TipoFase = entity.TipoFase,
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
              GiorniPrevisti = f.GiorniPrevisti ?? 0,
              TipoFase = f.TipoFase,
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
                GiorniPrevisti = f.GiorniPrevisti ?? 0,
                TipoFase = f.TipoFase,
                Stato = f.Stato
            })
            .ToListAsync(ct);

        return list;
    }

    public async Task<bool> UpdateAsync(UpdateFaseProgettoDto dto, CancellationToken ct = default)
    {
        var affected = await db.FasiProgetto
            .Where(f => f.Id == dto.Id && f.MacroFaseId == dto.MacroFaseId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(f => f.Nome, dto.Nome)
                .SetProperty(f => f.GiorniPrevisti, dto.GiorniPrevisti)
                .SetProperty(f => f.TipoFase, dto.TipoFase)
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
