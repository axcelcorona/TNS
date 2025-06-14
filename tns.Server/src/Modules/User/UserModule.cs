using Microsoft.EntityFrameworkCore;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;
using tns.Server.src.Modules.User.Infrastructure.Data;
using tns.Server.src.Modules.User.Infrastructure.Repositories;
using tns.Server.src.Modules.User.Infrastructure.Services;

namespace tns.Server.src.Modules.User
{
    public static class UserModule
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Ensure the required extension methods are available
            services.AddUserDomain()
                    .AddUserApplication()
                    .AddUserInfrastructure(configuration);

            return services;
        }

        public static IApplicationBuilder UseUserModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
    public static class UserModuleExtensions
    {
        public static IServiceCollection AddUserDomain(this IServiceCollection services)
        {
            
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddUserApplication(this IServiceCollection services)
        {

            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UserModuleExtensions).Assembly));

            // Registrar otros servicios de aplicación
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }

        public static IServiceCollection AddUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("UserDb")));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();

            // Aplicar migraciones automáticamente (solo para desarrollo)
            //using var scope = services.BuildServiceProvider().CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            //dbContext.Database.Migrate();

            return services;
        }
    }
}
