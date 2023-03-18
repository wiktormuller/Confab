using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using FluentAssertions;

namespace Confab.Modules.Attendances.Tests.Unit.Entities;

public class Participant_Attend_Tests
{
    private readonly Participant _participant;

    private void Act(Attendance attendance)
        => _participant.Attend(attendance);

    public Participant_Attend_Tests() // Recreated per every test case
    {
        _participant = new Participant(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }

    [Fact]
    public void given_no_colliding_attendances_attend_should_succeed()
    {
        // Arrange
        var from = GetDate(9);
        var to = GetDate(10);
        var attendance = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);

        // Act
        Act(attendance);

        // Assert
        _participant.Attendances.Single().Should().Be(attendance);
    }

    [Fact]
    public void given_existing_attendance_with_same_agenda_item_attend_should_fail()
    {
        // Arrange
        var agendaItemId = Guid.NewGuid();
        var from = GetDate(9);
        var to = GetDate(10);
        var attendance1 = new Attendance(Guid.NewGuid(), agendaItemId, Guid.NewGuid(), _participant.Id, from, to);
        var attendance2 = new Attendance(Guid.NewGuid(), agendaItemId, Guid.NewGuid(), _participant.Id, from, to);
        _participant.Attend(attendance1);

        // Act
        var exception = Record.Exception(() => Act(attendance2));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<AlreadyParticipatingInEventException>();
    }

    [Theory]
    [MemberData(nameof(GetCollidingDates))]
    public void given_existing_attendance_with_time_collision_attend_should_fail(DateTime from, DateTime to)
    {
        // Arrange
        var existingAttendance = 
            new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);
        var nextAttendance =
            new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);

        _participant.Attend(existingAttendance);

        // Act
        var exception = Record.Exception(() => Act(nextAttendance));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<AlreadyParticipatingSameTimeException>();
    }

    [Theory]
    [MemberData(nameof(GetAvailableDates))]
    public void given_existing_attendance_without_time_collision_attend_should_succeed(DateTime from, DateTime to)
    {
        // Arrange
        var existingAttendance =
            new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);
        var nextAttendance =
            new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id,
            GetDate(9, 30), GetDate(11));

        _participant.Attend(existingAttendance);
        
        // Act
        Act(nextAttendance);

        // Assert
        _participant.Attendances.Last().Should().Be(nextAttendance);
    }

    private static DateTime GetDate(int hour, int minute = 0, int second = 0)
        => new(2023, 1, 1, hour, minute, second);

    private static IEnumerable<object[]> GetCollidingDates()
    {
        yield return new object[] { GetDate(9), GetDate(10) };
        yield return new object[] { GetDate(9), GetDate(11, 30) };
        yield return new object[] { GetDate(10), GetDate(10, 30) };
        yield return new object[] { GetDate(10), GetDate(12) };
    }

    private static IEnumerable<object[]> GetAvailableDates()
    {
        yield return new object[] { GetDate(8), GetDate(9) };
        yield return new object[] { GetDate(8), GetDate(9, 30) };
        yield return new object[] { GetDate(11), GetDate(12) };
        yield return new object[] { GetDate(12), GetDate(13) };
    }
}
