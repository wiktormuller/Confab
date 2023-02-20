using Confab.Shared.Abstractions.Messaging;
using System.Threading.Channels;

namespace Confab.Shared.Infrastructure.Messaging.Dispatchers;

internal interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}
