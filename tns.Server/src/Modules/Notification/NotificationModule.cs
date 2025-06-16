using Microsoft.Extensions.Configuration;
using tns.Server.src.Modules.Notification.Domain;
using tns.Server.src.Modules.Notification.Domain.Repositories;
using tns.Server.src.Modules.Notification.Infrastructure.Smtp;

namespace tns.Server.src.Modules.Notification
{
    public static class NotificationModule
    {

        public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
        {

            

            services.AddNotificacionDomain()
                    .AddNotificacionAplication(configuration)
                    .AddNotificacionInfrastructure(configuration);

            return services;
        }

    }

    public static class NotificationModuleExtensions
    {
        public static IServiceCollection AddNotificacionDomain(this IServiceCollection services)
        {

            services.AddScoped<INotificationSender, SmtpNotificationSender>();
            
            return services;
        }
        public static IServiceCollection AddNotificacionAplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
        public static IServiceCollection AddNotificacionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }

}
