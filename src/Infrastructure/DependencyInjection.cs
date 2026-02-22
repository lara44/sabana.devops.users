using Domain.Aggregates.Users.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Registrar repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}
