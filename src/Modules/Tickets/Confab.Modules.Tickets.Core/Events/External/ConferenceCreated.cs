using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Tickets.Core.Events.External;

// Event by definition should be immutable, because we don't want to change past
public record ConferenceCreated(Guid Id, string Name, int? ParticipantsLimit, DateTime From, DateTime To) : IEvent;