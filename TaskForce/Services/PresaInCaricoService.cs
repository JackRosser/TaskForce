using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Enum;
using TaskForce.Models;

namespace TaskForce.Services;

public class PresaInCaricoService(AppDbContext db) : IPresaInCaricoService
{
    public async Task<IEnumerable<PresaInCaricoDto>> GetByFaseAsync(int faseProgettoId, CancellationToken ct = default)
    {
        var list = await db.PreseInCarico
            .AsNoTracking()
            .Where(p => p.FaseProgettoId == faseProgettoId)
            .OrderByDescending(p => p.DataPresaInCarico)
            .Select(p => new PresaInCaricoDto
            {
                Id = p.Id,
                FaseProgettoId = p.FaseProgettoId,
                UserId = p.UserId,
                DataPresaInCarico = p.DataPresaInCarico,
                DataFineLavoro = p.DataFineLavoro
            })
            .ToListAsync(ct);

        return list;
    }

    public Task<PresaInCaricoDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.PreseInCarico
          .AsNoTracking()
          .Where(p => p.Id == id)
          .Select(p => new PresaInCaricoDto
          {
              Id = p.Id,
              FaseProgettoId = p.FaseProgettoId,
              UserId = p.UserId,
              DataPresaInCarico = p.DataPresaInCarico,
              DataFineLavoro = p.DataFineLavoro
          })
          .FirstOrDefaultAsync(ct);

    public async Task<PresaInCaricoDto> AssegnaAsync(int faseProgettoId, int userId, CancellationToken ct = default)
    {
        // Verifiche esistenza (opzionali ma consigliate)
        var faseExists = await db.FasiProgetto.AnyAsync(f => f.Id == faseProgettoId, ct);
        if (!faseExists) throw new KeyNotFoundException("FaseProgetto non trovata.");

        var userExists = await db.Users.AnyAsync(u => u.Id == userId, ct);
        if (!userExists) throw new KeyNotFoundException("User non trovato.");

        using var tx = await db.Database.BeginTransactionAsync(ct);

        var now = DateTime.Now; // come richiesto
        var entity = new PresaInCarico
        {
            FaseProgettoId = faseProgettoId,
            UserId = userId,
            DataPresaInCarico = now,
            DataFineLavoro = null
        };

        db.PreseInCarico.Add(entity);
        await db.SaveChangesAsync(ct);

        // Aggiorna lo stato della fase
        await db.FasiProgetto
            .Where(f => f.Id == faseProgettoId)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.Stato, StatoFase.PresoInCarico), ct);

        await tx.CommitAsync(ct);

        return new PresaInCaricoDto
        {
            Id = entity.Id,
            FaseProgettoId = entity.FaseProgettoId,
            UserId = entity.UserId,
            DataPresaInCarico = entity.DataPresaInCarico,
            DataFineLavoro = entity.DataFineLavoro
        };
    }
}
