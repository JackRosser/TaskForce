using TaskForce.Dto.User;

namespace TaskForce.Services
{
    public interface IUserService
    {
        Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<GetUserDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<GetUserDto>> GetAllAsync(CancellationToken ct = default);
        Task<bool> UpdateAsync(UpdateUserDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<GetUserDto>> GetUtentiNonAssegnatiAllaFaseAsync(int id, CancellationToken ct = default);

    }
}
