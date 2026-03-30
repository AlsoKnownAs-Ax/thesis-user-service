using UserService.Application.Interfaces;
using UserService.Domain;

namespace UserService.Application.Services;

public class UserApplicationService
{
    private readonly IUserRepository _userRepository;

    public UserApplicationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }  

    public async Task<User> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if(existing != null)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByIdAsync(id, cancellationToken);
    }
}