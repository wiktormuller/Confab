using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Shared.Infrastructure.Messaging.Brokers;

// It's a wrapper for IModuleClient
internal sealed class InMemoryMessageBroker : IMessageBroker
{
    private readonly IModuleClient _moduleClient;

    public InMemoryMessageBroker(IModuleClient moduleClient)
    {
        _moduleClient = moduleClient;
    }

    public async Task PublishAsync(params IMessage[] messages)
    {
        if (messages is null)
        {
            return;
        }

        messages = messages.Where(x => x is not null).ToArray();

        if (!messages.Any())
        {
            return;
        }

        var tasks = new List<Task>();

        foreach (var message in messages)
        {
            tasks.Add(_moduleClient.PublishAsync(message));
        }

        await Task.WhenAll(tasks);
    }
}
