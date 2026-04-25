using Fitonyashka.InfrastructureLayer.Interfaces;
using Fytonyashka.DataAccessLayer.Repositories;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;

namespace Fitonyashka.InfrastructureLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnitityServices(this IServiceCollection services) {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IWeightService, WeightService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IStaticFilePublisher, StaticFilePublisher>();
        services.AddSingleton<IWeightDateRangeService, WeightDateRangeService>();
        services.AddSingleton<IUserGoalService, UserGoalService>();
        services.AddSingleton<ISleepService, SleepService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddTransient<IWeightRepository, WeightRepository>();
        services.AddTransient<IUserGoalRepository, UserGoalRepository>();
        services.AddTransient<IWeightDateRangeRepository, WeightDateRangeRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ISleepRepository, SleepRepository>();

        return services;
    }

    public static IServiceCollection AddUserContext(this IServiceCollection services) {
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();

        return services;
    }
}
