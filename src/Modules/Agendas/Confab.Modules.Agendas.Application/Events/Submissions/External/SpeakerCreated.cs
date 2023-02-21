using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Agendas.Application.Events.Submissions.External;

public record SpeakerCreated(Guid Id, string FullName) : IEvent;
