using System.Security.Cryptography;
using System.Text.Json.Serialization;
using EShopFanerum.Core.Helpers;
using MassTransit;

namespace EShopFanerum.ManagmentService.Consumer;

/// <summary>
/// Расширения для подключения сервисов подсистемы предотвращения мошенничества.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение MassTransit.
    /// </summary>
    /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <param name="settings"></param>
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services,
        RabbitMqSettings settings)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrdersConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(settings.HostNames, settings.VirtualHost, h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        return services;
    }
}