namespace Domain.Aggregates.Users.Repositories;

public interface IUserRepository
{
    Task<List<UserRoot>> GetAllUsersAsync();
}
