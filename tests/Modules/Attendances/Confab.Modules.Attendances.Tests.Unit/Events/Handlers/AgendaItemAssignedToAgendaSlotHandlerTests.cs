using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Application.Events.External;
using Confab.Modules.Attendances.Application.Events.External.Handlers;
using Confab.Modules.Attendances.Application.Exceptions;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Policies;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Modules.Attendances.Domain.Types;
using Confab.Shared.Abstractions.Events;
using FluentAssertions;
using Moq;

namespace Confab.Modules.Attendances.Tests.Unit.Events.Handlers;

public class AgendaItemAssignedToAgendaSlotHandlerTests
{
    private readonly Mock<IAttendableEventsRepository> _attendableEventsRepository;
    private readonly Mock<IAgendasApiClient> _agendasApiClient;
    private readonly Mock<ISlotPolicyFactory> _slotPolicyFactory;
    private readonly Mock<ISlotPolicy> _slotPolicy;

    private readonly IEventHandler<AgendaItemAssignedToAgendaSlot> _handler;

    private Task Act(AgendaItemAssignedToAgendaSlot @event)
        => _handler.HandleAsync(@event);

    public AgendaItemAssignedToAgendaSlotHandlerTests()
    {
        _attendableEventsRepository = new Mock<IAttendableEventsRepository>();
        _agendasApiClient = new Mock<IAgendasApiClient>();
        _slotPolicyFactory = new Mock<ISlotPolicyFactory>();
        _slotPolicy = new Mock<ISlotPolicy>();
        _handler = new AgendaItemAssignedToAgendaSlotHandler(_attendableEventsRepository.Object, _agendasApiClient.Object, _slotPolicyFactory.Object);
    }

    [Fact]
    public async Task given_already_existing_attendable_event_new_one_should_not_be_added()
    {
        // Arrange
        var attendableEvent = GetAttendableEvent();
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), attendableEvent.Id);
        _attendableEventsRepository.Setup(x => x.GetAsync(It.Is<AttendableEventId>(x => x == @event.AgendaItemId))).ReturnsAsync(attendableEvent);

        // Act
        await Act(@event);

        // Assert
        _attendableEventsRepository.Verify(x => x.GetAsync(It.Is<AttendableEventId>(x => x.Value == @event.AgendaItemId)), Times.Once);
        _agendasApiClient.Verify(x => x.GetRegularAgendaSlotAsync(default), Times.Never);
        _attendableEventsRepository.Verify(x => x.AddAsync(It.IsAny<AttendableEvent>()), Times.Never);
    }

    [Fact]
    public async Task given_missing_regular_agenda_slot_handler_should_fail()
    {
        // Arrange
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var exception = await Record.ExceptionAsync(() => Act(@event));

        // Arrange
        exception.Should().NotBeNull();
        exception.Should().BeOfType<AttendableEventNotFoundException>();
        _attendableEventsRepository.Verify(x => x.GetAsync(It.Is<AttendableEventId>(x => x == @event.AgendaItemId)), Times.Once);
        _agendasApiClient.Verify(x => x.GetRegularAgendaSlotAsync(It.Is<Guid>(x => x == @event.AgendaItemId)), Times.Once);
    }

    [Fact]
    public async Task given_no_participants_limit_attendable_event_should_not_be_added()
    {
        // Arrange
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());
        var agendaSlotDto = GetRegularAgendaSlotDto();
        _agendasApiClient.Setup(x => x.GetRegularAgendaSlotAsync(It.Is<Guid>(x => x == @event.AgendaItemId))).ReturnsAsync(agendaSlotDto);

        // Act
        await Act(@event);

        // Assert
        _attendableEventsRepository.Verify(x => x.GetAsync(It.Is<AttendableEventId>(x => x == @event.AgendaItemId)), Times.Once);
        _agendasApiClient.Verify(x => x.GetRegularAgendaSlotAsync(It.Is<Guid>(x => x == @event.AgendaItemId)), Times.Once);
        _attendableEventsRepository.Verify(x => x.AddAsync(It.IsAny<AttendableEvent>()), Times.Never);
    }

    [Fact]
    public async Task given_participants_limit_attendable_event_should_be_added()
    {
        // Arrange
        const int participantsLimit = 100;
        var slots = Enumerable.Range(0, participantsLimit).Select(x => new Slot(Guid.NewGuid()));
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());
        var agendaSlotDto = GetRegularAgendaSlotDto(participantsLimit);
        var tags = agendaSlotDto.AgendaItem.Tags.ToArray();

        _agendasApiClient.Setup(x => x.GetRegularAgendaSlotAsync(It.Is<Guid>(x => x == @event.AgendaItemId))).ReturnsAsync(agendaSlotDto);
        _slotPolicyFactory.Setup(x => x.Create(It.Is<string[]>(x => x.SequenceEqual(tags)))).Returns(_slotPolicy.Object);
        _slotPolicy.Setup(x => x.Generate(It.Is<int>(x => x == participantsLimit))).Returns(slots);

        // Act
        await Act(@event);

        // Assert
        _attendableEventsRepository.Verify(x => x.GetAsync(It.Is<AttendableEventId>(x => x == @event.AgendaItemId)), Times.Once);
        _agendasApiClient.Verify(x => x.GetRegularAgendaSlotAsync(It.Is<Guid>(x => x == @event.AgendaItemId)), Times.Once);
        _slotPolicyFactory.Verify(x => x.Create(It.Is<string[]>(x => x.SequenceEqual(tags))), Times.Once);
        _slotPolicy.Verify(x => x.Generate(It.Is<int>(x => x == participantsLimit)), Times.Once);
        _attendableEventsRepository.Verify(x => x.AddAsync(It.Is<AttendableEvent>(x =>
            x.Id == @event.AgendaItemId &&
            x.ConferenceId == agendaSlotDto.AgendaItem.ConferenceId &&
            x.From == agendaSlotDto.From &&
            x.To == agendaSlotDto.To)), Times.Once);
    }

    private static AttendableEvent GetAttendableEvent()
        => new(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

    private static RegularAgendaSlotDto GetRegularAgendaSlotDto(int? participantsLimit = null)
        => new()
        {
            Id = Guid.NewGuid(),
            ParticipantsLimit = participantsLimit,
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(1),
            AgendaItem = new AgendaItemDto
            {
                ConferenceId = Guid.NewGuid(),
                Tags = new[] { "tag1", "tag2" }
            }
        };
}
