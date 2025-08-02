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

    public async Task UpdateAsync(int id, UpdateFaseProgettoDto dto, CancellationToken ct)
    {
        var fase = await db.FasiProgetto.FindAsync([id], ct);
        if (fase is null)
            throw new KeyNotFoundException($"FaseProgetto con id {id} non trovata");

        fase.Nome = dto.Nome;
        fase.GiorniPrevisti = dto.GiorniPrevisti;
        fase.TipoFase = dto.TipoFase;
        fase.Stato = dto.Stato;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var fase = await db.FasiProgetto.FindAsync([id], ct);
        if (fase is null)
            throw new KeyNotFoundException($"FaseProgetto con id {id} non trovata");

        db.FasiProgetto.Remove(fase);
        await db.SaveChangesAsync(ct);
    }
}
