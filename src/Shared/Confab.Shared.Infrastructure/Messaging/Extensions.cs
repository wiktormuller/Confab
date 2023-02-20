using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Infrastructure.Messaging.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Messaging;

internal static class Extensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();

        return services;
    }
}
