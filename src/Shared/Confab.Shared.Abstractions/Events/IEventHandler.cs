namespace Confab.Shared.Abstractions.Events;

public interface IEventHandler<in TEvent> where TEvent : class, IEvent
{
    Task HandleAsync(TEvent @event); // It's contravariant argument thanks to 'in TEvent'
}