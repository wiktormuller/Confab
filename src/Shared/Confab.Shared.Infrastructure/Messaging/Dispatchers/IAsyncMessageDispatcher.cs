using Confab.Shared.Abstractions.Messaging;
namespace Confab.Shared.Infrastructure.Messaging.Dispatchers;

internal interface IAsyncMessageDispatcher
{
    public Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
}
