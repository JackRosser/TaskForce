using Microsoft.EntityFrameworkCore;
using TaskForce.Database;
using TaskForce.Dto.User;
using TaskForce.Models;

namespace TaskForce.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var entity = new User { Nome = dto.Nome! };
        db.Users.Add(entity);
        await db.SaveChangesAsync(ct);

        return new GetUserDto { Id = entity.Id, Nome = entity.Nome };
    }

    public Task<GetUserDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Users
          .AsNoTracking()
          .Where(u => u.Id == id)
          .Select(u => new GetUserDto { Id = u.Id, Nome = u.Nome })
          .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<GetUserDto>> GetAllAsync(CancellationToken ct = default) =>
    await db.Users
        .AsNoTracking()
        .OrderBy(u => u.Id)
        .Select(u => new GetUserDto { Id = u.Id, Nome = u.Nome })
        .ToListAsync(ct);



    public async Task<bool> UpdateAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        var affected = await db.Users
            .Where(u => u.Id == dto.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.Nome, dto.Nome!), ct);

        return affected == 1;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var affected = await db.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(ct);

        return affected == 1;
    }
}
