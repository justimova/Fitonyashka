using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.Services;
using Fitonyashka.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Repositories;

namespace Fitonyashka.InfrastructureLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnitityServices(this IServiceCollection services) {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWeightService, WeightService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IStaticFilePublisher, StaticFilePublisher>();
        services.AddScoped<IWeightDateRangeService, WeightDateRangeService>();
        services.AddScoped<IUserGoalService, UserGoalService>();
        services.AddScoped<ISleepService, SleepService>();
        services.AddScoped<IBmiService, BmiService>();

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
