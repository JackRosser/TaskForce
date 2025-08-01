using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
// sposta l'using del tuo PausaDto nel namespace giusto:
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Enum;
using TaskForce.Models;

namespace TaskForce.Services;

public class PausaService(AppDbContext db) : IPausaService
{
    public async Task<PausaDto> StartAsync(int presaInCaricoId, int userId, CancellationToken ct = default)
    {
        // Verifiche base
        var presa = await db.PreseInCarico.FirstOrDefaultAsync(p => p.Id == presaInCaricoId, ct)
                   ?? throw new KeyNotFoundException("PresaInCarico non trovata.");

        // Evita doppie pause aperte sulla stessa presa
        var hasOpen = await db.Pause.AnyAsync(x => x.PresaInCaricoId == presaInCaricoId && x.DataFine == null, ct);
        if (hasOpen) throw new InvalidOperationException("Esiste già una pausa aperta per questa presa.");

        var now = DateTime.Now;
        var pausa = new Pausa
        {
            UserId = userId,
            PresaInCaricoId = presaInCaricoId,
            DataInizio = now,
            DataFine = null
        };

        using var tx = await db.Database.BeginTransactionAsync(ct);

        db.Pause.Add(pausa);
        await db.SaveChangesAsync(ct);

        // Stato -> InPausa
        await db.FasiProgetto
            .Where(f => f.Id == presa.FaseProgettoId)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.Stato, StatoFase.InPausa), ct);

        await tx.CommitAsync(ct);

        return new PausaDto
        {
            Id = pausa.Id,
            UserId = pausa.UserId,
            PresaInCaricoId = pausa.PresaInCaricoId,
            DataInizio = pausa.DataInizio,
            DataFine = pausa.DataFine
        };
    }

    public async Task<bool> StopAsync(int pausaId, int userId, CancellationToken ct = default)
    {
        // Recupera pausa + presa per risalire alla fase
        var pausa = await db.Pause
            .Include(p => p) // no-op, ma lascio per chiarezza
            .FirstOrDefaultAsync(p => p.Id == pausaId, ct);

        if (pausa is null) return false;
        if (pausa.DataFine is not null) return true; // già chiusa, idempotente

        // Risali alla presa e alla fase
        var presa = await db.PreseInCarico.FirstOrDefaultAsync(x => x.Id == pausa.PresaInCaricoId, ct);
        if (presa is null) return false;

        var now = DateTime.Now;

        using var tx = await db.Database.BeginTransactionAsync(ct);

        // Chiudi la pausa
        var affectedPause = await db.Pause
            .Where(p => p.Id == pausaId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.DataFine, now)
                .SetProperty(p => p.UserId, userId) // se vuoi tracciare chi l'ha chiusa
            , ct);

        if (affectedPause != 1)
        {
            await tx.RollbackAsync(ct);
            return false;
        }

        // Stato -> PresoInCarico
        await db.FasiProgetto
            .Where(f => f.Id == presa.FaseProgettoId)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.Stato, StatoFase.PresoInCarico), ct);

        await tx.CommitAsync(ct);
        return true;
    }

    public async Task<IEnumerable<PausaDto>> GetByPresaAsync(int presaInCaricoId, CancellationToken ct = default)
    {
        var items = await db.Pause
            .AsNoTracking()
            .Where(p => p.PresaInCaricoId == presaInCaricoId)
            .OrderByDescending(p => p.DataInizio)
            .Select(p => new PausaDto
            {
                Id = p.Id,
                UserId = p.UserId,
                PresaInCaricoId = p.PresaInCaricoId,
                DataInizio = p.DataInizio,
                DataFine = p.DataFine
            })
            .ToListAsync(ct);

        return items;
    }
}
