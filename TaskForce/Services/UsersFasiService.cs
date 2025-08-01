using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.User;
using TaskForce.Enum;

namespace TaskForce.Services;

public class UsersFasiService(AppDbContext db) : IUsersFasiService
{
    // Fasi correnti per tutti gli utenti (ultima PresaInCarico senza DataFineLavoro)
    public async Task<IEnumerable<UserFaseCorrenteDto>> GetFasiCorrentiAsync(CancellationToken ct = default)
    {
        var lastByUser =
            db.PreseInCarico
              .Where(p => p.DataFineLavoro == null)
              .GroupBy(p => p.UserId)
              .Select(g => new { UserId = g.Key, LastStart = g.Max(x => x.DataPresaInCarico) });

        var query =
            from k in lastByUser
            join p in db.PreseInCarico on new { k.UserId, k.LastStart } equals new { p.UserId, LastStart = p.DataPresaInCarico }
            join f in db.FasiProgetto on p.FaseProgettoId equals f.Id
            join m in db.MacroFasi on f.MacroFaseId equals m.Id
            join u in db.Users on p.UserId equals u.Id
            join pr in db.Progetti on m.ProgettoId equals pr.Id into prj
            from pr in prj.DefaultIfEmpty()
            select new UserFaseCorrenteDto
            {
                UserId = u.Id,
                UserNome = u.Nome,
                FaseProgettoId = f.Id,
                FaseNome = f.Nome,
                ProgettoId = m.ProgettoId,
                ProgettoNome = pr != null ? pr.Nome : null,
                DataPresaInCarico = p.DataPresaInCarico
            };

        return await query
            .OrderBy(x => x.UserId)
            .ToListAsync(ct);
    }

    // Fasi completate per un utente (ha DataFineLavoro valorizzata)
    public async Task<IEnumerable<UserFaseDto>> GetFasiCompletateByUserAsync(int userId, CancellationToken ct = default)
    {
        var query =
            from p in db.PreseInCarico.AsNoTracking()
            where p.UserId == userId && p.DataFineLavoro != null
            join f in db.FasiProgetto on p.FaseProgettoId equals f.Id
            join m in db.MacroFasi on f.MacroFaseId equals m.Id
            join pr in db.Progetti on m.ProgettoId equals pr.Id into prj
            from pr in prj.DefaultIfEmpty()
            orderby p.DataFineLavoro descending
            select new UserFaseDto
            {
                FaseProgettoId = f.Id,
                FaseNome = f.Nome,
                ProgettoId = m.ProgettoId,
                ProgettoNome = pr != null ? pr.Nome : null,
                DataPresaInCarico = p.DataPresaInCarico,
                DataFineLavoro = p.DataFineLavoro!.Value
            };

        return await query.ToListAsync(ct);
    }

    // Overview singolo utente: fase corrente (se c'è) + completate
    public async Task<UserFasiOverviewDto?> GetOverviewByUserAsync(int userId, CancellationToken ct = default)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return null;

        // Fase corrente (ultima senza DataFineLavoro)
        var currentKey = await db.PreseInCarico
            .Where(p => p.UserId == userId && p.DataFineLavoro == null)
            .GroupBy(p => p.UserId)
            .Select(g => new { LastStart = g.Max(x => x.DataPresaInCarico) })
            .FirstOrDefaultAsync(ct);

        UserFaseCorrenteDto? corrente = null;
        if (currentKey is not null)
        {
            corrente = await (
                from p in db.PreseInCarico
                where p.UserId == userId && p.DataFineLavoro == null && p.DataPresaInCarico == currentKey.LastStart
                join f in db.FasiProgetto on p.FaseProgettoId equals f.Id
                join m in db.MacroFasi on f.MacroFaseId equals m.Id
                join pr in db.Progetti on m.ProgettoId equals pr.Id into prj
                from pr in prj.DefaultIfEmpty()
                select new UserFaseCorrenteDto
                {
                    UserId = user.Id,
                    UserNome = user.Nome,
                    FaseProgettoId = f.Id,
                    FaseNome = f.Nome,
                    ProgettoId = m.ProgettoId,
                    ProgettoNome = pr != null ? pr.Nome : null,
                    DataPresaInCarico = p.DataPresaInCarico
                }
            ).FirstOrDefaultAsync(ct);
        }

        var completate = await GetFasiCompletateByUserAsync(userId, ct);

        return new UserFasiOverviewDto
        {
            UserId = user.Id,
            UserNome = user.Nome,
            Corrente = corrente,
            Completate = completate
        };
    }

    public async Task<bool> CompletaFaseAsync(int faseProgettoId, CancellationToken ct = default)
    {
        using var tx = await db.Database.BeginTransactionAsync(ct);

        var now = DateTime.Now;

        // chiudi eventuali prese in carico ancora aperte sulla fase
        await db.PreseInCarico
            .Where(p => p.FaseProgettoId == faseProgettoId && p.DataFineLavoro == null)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.DataFineLavoro, now), ct);

        // imposta lo stato della fase a Completato
        var affected = await db.FasiProgetto
            .Where(f => f.Id == faseProgettoId)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.Stato, StatoFase.Completato), ct);

        if (affected != 1)
        {
            await tx.RollbackAsync(ct);
            return false;
        }

        await tx.CommitAsync(ct);
        return true;
    }
}
