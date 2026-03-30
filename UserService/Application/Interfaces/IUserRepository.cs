using UserService.Domain;

namespace UserService.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}