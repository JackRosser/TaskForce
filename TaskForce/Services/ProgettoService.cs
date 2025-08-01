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
}
