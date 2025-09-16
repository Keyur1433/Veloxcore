using APZMS.Application.Interfaces;
using APZMS.Infrastructure.Database;
using APZMS.Infrastructure.Repositories;
using APZMS.Infrastructure.Repositories.Interfaces;
using APZMS.Infrastructure.Security;
using APZMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APZMS.Infrastructure;

public static class RegistrationExtension
{
    public static void RegisterInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportRepository, ReportRepository>();
    }
}
