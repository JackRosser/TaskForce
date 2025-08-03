using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Enum;
using TaskForce.Models;

public class PresaInCaricoService(AppDbContext db) : IPresaInCaricoService
{
    private const int OrePerGiorno = 8;

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

    public async Task PausaAsync(int id, CancellationToken ct = default)
    {
        var presa = await db.PreseInCarico.FindAsync([id], ct)
            ?? throw new KeyNotFoundException("Presa in carico non trovata");

        if (presa.Stato != StatoUtente.Attivo)
            throw new InvalidOperationException("Non puoi mettere in pausa una lavorazione non attiva");

        // Apri nuova pausa
        var pausa = new Pausa
        {
            PresaInCaricoId = presa.Id,
            DataInizio = DateTime.Now
        };

        db.Pause.Add(pausa);
        presa.Stato = StatoUtente.InPausa;
        await db.SaveChangesAsync(ct);
    }

    public async Task RiprendiAsync(int id, CancellationToken ct = default)
    {
        var presa = await db.PreseInCarico.FindAsync([id], ct)
            ?? throw new KeyNotFoundException("Presa in carico non trovata");

        if (presa.Stato != StatoUtente.InPausa)
            throw new InvalidOperationException("La lavorazione non è in pausa");

        // Chiudi l'ultima pausa aperta
        var pausa = await db.Pause
            .Where(p => p.PresaInCaricoId == presa.Id && p.DataFine == null)
            .OrderByDescending(p => p.DataInizio)
            .FirstOrDefaultAsync(ct);

        if (pausa is null)
            throw new InvalidOperationException("Nessuna pausa aperta trovata");

        pausa.DataFine = DateTime.Now;
        presa.Stato = StatoUtente.Attivo;
        await db.SaveChangesAsync(ct);
    }

    public async Task ConcludiAsync(int id, CancellationToken ct = default)
    {
        var presa = await db.PreseInCarico.FindAsync([id], ct)
            ?? throw new KeyNotFoundException("Presa in carico non trovata");

        presa.Stato = StatoUtente.Concluso;
        presa.DataFineLavoro = DateTime.Now;

        // Se tutti hanno concluso, aggiorna la fase
        var tuttePrese = await db.PreseInCarico
            .Where(p => p.FaseProgettoId == presa.FaseProgettoId)
            .ToListAsync(ct);

        if (tuttePrese.All(p => p.Stato == StatoUtente.Concluso))
        {
            var fase = await db.FasiProgetto.FindAsync([presa.FaseProgettoId], ct);
            if (fase is not null)
            {
                fase.Stato = StatoFase.Completato;
            }
        }

        await db.SaveChangesAsync(ct);
    }


    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var presa = await db.PreseInCarico.FindAsync([id], ct);
        if (presa is null)
            throw new KeyNotFoundException($"Presa in carico con id {id} non trovata");

        var faseId = presa.FaseProgettoId;

        db.PreseInCarico.Remove(presa);
        await db.SaveChangesAsync(ct);

        // Se non ci sono più lavorazioni per la fase, riporta a DaCompletare
        bool nessunaLavorazione = !await db.PreseInCarico
            .AnyAsync(p => p.FaseProgettoId == faseId, ct);

        if (nessunaLavorazione)
        {
            var fase = await db.FasiProgetto
                .FirstOrDefaultAsync(f => f.Id == faseId, ct);

            if (fase is not null && fase.Stato == StatoFase.Completato)
            {
                fase.Stato = StatoFase.DaCompletare;
                await db.SaveChangesAsync(ct);
            }
        }
    }


}
