using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using FluentAssertions;

namespace Confab.Modules.Attendances.Tests.Unit.Entities;

public class AttendableEvent_Attend_Tests
{
    private readonly Guid _conferenceId = Guid.NewGuid();
    private readonly AttendableEvent _attendableEvent;

    private Attendance Act(Participant participant)
        => _attendableEvent.Attend(participant);

    public AttendableEvent_Attend_Tests()
    {
        _attendableEvent = new AttendableEvent(Guid.NewGuid(), _conferenceId,
                new DateTime(2020, 4, 1, 9, 0, 0),
                new DateTime(2020, 4, 1, 10, 0, 0));
    }

    [Fact]
    public void given_no_slots_attend_should_fail()
    {
        // Arrange
        var participant = GetParticipant();

        // Act
        var exception = Record.Exception(() => Act(participant));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<NoFreeSlotsException>();
        _attendableEvent.Slots.Should().BeEmpty();
    }

    [Fact]
    public void given_existing_slot_with_same_participant_attend_should_fail()
    {
        // Arrange
        var participant = GetParticipant();
        _attendableEvent.AddSlots(new Slot(Guid.NewGuid(), participant.Id));

        // Act
        var exception = Record.Exception(() => Act(participant));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<AlreadyParticipatingInEventException>();
    }

    [Fact]
    public void given_no_free_slots_attend_should_fail()
    {
        // Arrange
        var participant1 = GetParticipant();
        var participant2 = GetParticipant();

        // Act
        var exception = Record.Exception(() => Act(participant2));

        // Arrange
        exception.Should().NotBeNull();
        exception.Should().BeOfType<NoFreeSlotsException>();
    }

    [Fact]
    public void given_free_slots_attend_should_succeed()
    {
        // Arrange
        var participant = GetParticipant();
        var slot = new Slot(Guid.NewGuid());
        _attendableEvent.AddSlots(slot);

        // Act
        var attendance = Act(participant);
        attendance.Should().NotBeNull();

        // Assert
        attendance.ParticipantId.Should().Be(participant.Id);
        attendance.SlotId.Should().Be(slot.Id);
    }


    private Participant GetParticipant()
        => new(Guid.NewGuid(), _conferenceId, Guid.NewGuid());
}
