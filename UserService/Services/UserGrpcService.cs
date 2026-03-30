using Grpc.Core;
using UserService.Application.Services;
using UserService.Grpc;

namespace UserService.Services;

public class UserGrpcService : Grpc.UserService.UserServiceBase
{
    private readonly UserApplicationService _userApplicationService;

    public UserGrpcService(UserApplicationService userApplicationService)
    {
        _userApplicationService = userApplicationService;
    }

    public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _userApplicationService.CreateUserAsync(
                request.Email,
                request.FirstName,
                request.LastName,
                context.CancellationToken
            );

            return new UserResponse
            {
                User = MapUser(user)
            };
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
    }

    public override async Task<UserResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user id."));

        var user = await _userApplicationService.GetUserByIdAsync(id, context.CancellationToken);

        if (user == null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found."));

        return new UserResponse
        {
            User = MapUser(user)
        };
    }

    private static UserService.Grpc.User MapUser(Domain.User user)
    {
        return new UserService.Grpc.User
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt.ToString("O"),
            UpdatedAt = user.UpdatedAt.ToString("O")
        };
    }
}