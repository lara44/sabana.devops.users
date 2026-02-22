using Domain.Aggregates.Users;
using Domain.Aggregates.Users.Repositories;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<List<UserRoot>> GetAllUsersAsync()
    {
        var users = new List<UserRoot>
        {
            new UserRoot
            {
                Id = 1,
                Name = "Juan Pérez",
                Email = "juan.perez@example.com",
                Role = "Administrator",
                IsActive = true
            },
            new UserRoot
            {
                Id = 2,
                Name = "María García",
                Email = "maria.garcia@example.com",
                Role = "Developer",
                IsActive = true
            },
            new UserRoot
            {
                Id = 3,
                Name = "Carlos Rodríguez",
                Email = "carlos.rodriguez@example.com",
                Role = "Developer",
                IsActive = true
            },
            new UserRoot
            {
                Id = 4,
                Name = "Ana Martínez",
                Email = "ana.martinez@example.com",
                Role = "Manager",
                IsActive = false
            },
            new UserRoot
            {
                Id = 5,
                Name = "Luis Sánchez",
                Email = "luis.sanchez@example.com",
                Role = "Analyst",
                IsActive = true
            }
        };

        return Task.FromResult(users);
    }
}
