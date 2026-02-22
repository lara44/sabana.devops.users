using AtcMediator;
using Domain.Aggregates.Users.Repositories;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IAtcRequestHandler<GetAllUsersQuery, List<GetAllUsersQueryResult>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<GetAllUsersQueryResult>> HandleAsync(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllUsersAsync();
        
        return users.Select(user => new GetAllUsersQueryResult
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive
        }).ToList();
    }
}
