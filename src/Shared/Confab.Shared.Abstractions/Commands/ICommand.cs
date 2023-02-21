using Confab.Shared.Abstractions.Messaging;

namespace Confab.Shared.Abstractions.Commands;

// Marker interface
public interface ICommand : IMessage // It's IMessage because in the future we can send commands via message broker instead of events
{
}
