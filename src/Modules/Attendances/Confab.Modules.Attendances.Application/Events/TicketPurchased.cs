using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Attendances.Application.Events;

internal record TicketPurchased(Guid TicketId, Guid ConferenceId, Guid UserId) : IEvent;