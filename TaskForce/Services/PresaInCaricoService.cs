using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Enum;
using TaskForce.Models;

public class PresaInCaricoService(AppDbContext db) : IPresaInCaricoService
{
    public async Task<IEnumerable<PresaInCaricoRequest>> GetByFaseAsync(int faseProgettoId, CancellationToken ct = default)
    {
        return await db.PreseInCarico
            .AsNoTracking()
            .Where(p => p.FaseProgettoId == faseProgettoId)
            .OrderByDescending(p => p.DataPresaInCarico)
            .Select(p => new PresaInCaricoRequest
            {
                Id = p.Id,
                FaseProgettoId = p.FaseProgettoId,
                UserId = p.UserId,
                UserName = db.Users.FirstOrDefault(u => u.Id == p.UserId)!.Nome,
                DataPresaInCarico = p.DataPresaInCarico,
                DataFineLavoro = p.DataFineLavoro
            })
            .ToListAsync(ct);
    }

    public Task<PresaInCaricoRequest?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.PreseInCarico
          .AsNoTracking()
          .Where(p => p.Id == id)
          .Select(p => new PresaInCaricoRequest
          {
              Id = p.Id,
              FaseProgettoId = p.FaseProgettoId,
              UserId = p.UserId,
              UserName = db.Users.FirstOrDefault(u => u.Id == p.UserId)!.Nome,
              DataPresaInCarico = p.DataPresaInCarico,
              DataFineLavoro = p.DataFineLavoro
          })
          .FirstOrDefaultAsync(ct);

    public async Task<PresaInCaricoRequest> AssegnaAsync(int faseProgettoId, int userId, CancellationToken ct = default)
    {
        var now = DateTime.Now;

        var entity = new PresaInCarico
        {
            FaseProgettoId = faseProgettoId,
            UserId = userId,
            DataPresaInCarico = now,
            Stato = StatoUtente.Attivo
        };

        db.PreseInCarico.Add(entity);
        await db.SaveChangesAsync(ct);

        var user = await db.Users.FindAsync([userId], ct);

        return new PresaInCaricoRequest
        {
            Id = entity.Id,
            FaseProgettoId = entity.FaseProgettoId,
            UserId = entity.UserId,
            UserName = user?.Nome,
            DataPresaInCarico = entity.DataPresaInCarico
        };
    }

    public async Task<bool> EliminaAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.PreseInCarico.FindAsync([id], ct);
        if (entity is null) return false;

        db.PreseInCarico.Remove(entity);
        await db.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> MettiInPausaAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.PreseInCarico.FindAsync([id], ct);
        if (entity is null) return false;

        entity.Stato = StatoUtente.InPausa;
        await db.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> TerminaAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.PreseInCarico.FindAsync([id], ct);
        if (entity is null || entity.DataFineLavoro != null) return false;

        entity.DataFineLavoro = DateTime.Now;
        entity.Stato = StatoUtente.Attivo;

        await db.SaveChangesAsync(ct);

        // Aggiornamento stato fase se tutti hanno finito
        var faseId = entity.FaseProgettoId;

        var tutteTerminate = await db.PreseInCarico
            .Where(p => p.FaseProgettoId == faseId)
            .AllAsync(p => p.DataFineLavoro != null, ct);

        if (tutteTerminate)
        {
            var fase = await db.FasiProgetto.FindAsync([faseId], ct);
            if (fase != null)
            {
                fase.Stato = StatoFase.Completato;
                await db.SaveChangesAsync(ct);
            }
        }

        return true;
    }
}
