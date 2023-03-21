using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Application.DTO;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Infrastructure.EF;
using Confab.Modules.Attendances.Tests.Integration.Common;
using Confab.Shared.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Confab.Modules.Attendances.Tests.Integration.Controllers;

[Collection("integration")]
public class AttendancesControllerTests : IClassFixture<TestApplicationFactory>,
    IClassFixture<TestAttendancesDbContext> // ClassFixture used for constructor injection - injected before first test and disposed after last test
{
    private const string PATH = "attendances-module/attendances";
    private readonly Mock<IAgendasApiClient> _agendasApiClient;
    private HttpClient _httpClient;
    private AttendancesDbContext _dbContext;

    public AttendancesControllerTests(TestApplicationFactory factory, TestAttendancesDbContext dbContext)
    {
        _agendasApiClient = new Mock<IAgendasApiClient>();
        _httpClient = factory
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.AddSingleton(_agendasApiClient.Object); // Instead of real implementation from IoC Container we overwrite the implementation by Mock (last win)
            }))
            .CreateClient();
        _dbContext = dbContext.DbContext;
    }

    [Fact]
    public async Task get_browse_attendances_without_being_authorized_should_return_unauthorized_status_code()
    {
        // Arrange
        var conferenceId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"{PATH}/{conferenceId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_browse_attendances_given_invalid_participant_should_return_not_found()
    {
        // Arrange
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        Authenticate(userId);

        // Act
        var response = await _httpClient.GetAsync($"{PATH}/{conferenceId}");

        // Arrange
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task get_browse_attendances_given_valid_conference_and_participant_should_return_all_attendances()
    {
        // Arrange
        var from = DateTime.UtcNow;
        var to = from.AddDays(1);
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var participant = new Participant(Guid.NewGuid(), conferenceId, userId);
        var slot = new Slot(Guid.NewGuid(), participant.Id);
        var attendableEvent = new AttendableEvent(Guid.NewGuid(), conferenceId, from, to, new[] { slot });
        var attendance = new Attendance(Guid.NewGuid(), attendableEvent.Id, slot.Id, participant.Id, from, to);

        await _dbContext.AttendableEvents.AddAsync(attendableEvent);
        await _dbContext.Attendances.AddAsync(attendance);
        await _dbContext.Participants.AddAsync(participant);
        await _dbContext.Slots.AddAsync(slot);
        await _dbContext.SaveChangesAsync();

        var agendaTracks = new List<AgendaTrackDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Track 1",
                ConferenceId = conferenceId,
                Slots = new[]
                {
                    new RegularAgendaSlotDto
                    {
                        Id = Guid.NewGuid(),
                        From = from,
                        To = to,
                        AgendaItem = new AgendaItemDto
                        {
                            Id = attendableEvent.Id,
                            Title = "test",
                            Description = "test",
                            Level = 1
                        }
                    }
                }
            }
        };

        _agendasApiClient.Setup(x => x.GetAgendaAsync(It.Is<Guid>(x => x == conferenceId))).ReturnsAsync(agendaTracks);

        Authenticate(userId);

        // Act
        var response = await _httpClient.GetAsync($"{PATH}/{conferenceId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var attendances = await response.Content.ReadFromJsonAsync<AttendanceDto[]>();
        attendances.Should().NotBeEmpty();
        attendances.Length.Should().Be(1);
    }

    [Fact]
    public async Task post_attend_should_succeed_given_free_slots_and_valid_participants()
    {
        // Arrange
        var from = DateTime.UtcNow;
        var to = from.AddDays(1);
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var participant = new Participant(Guid.NewGuid(), conferenceId, userId);
        var slot = new Slot(Guid.NewGuid());
        var attendableEvent = new AttendableEvent(Guid.NewGuid(), conferenceId, from, to, new[] { slot });

        await _dbContext.AttendableEvents.AddAsync(attendableEvent);
        await _dbContext.Participants.AddAsync(participant);
        await _dbContext.Slots.AddAsync(slot);
        await _dbContext.SaveChangesAsync();

        Authenticate(userId);

        // Act
        var response = await _httpClient.PostAsJsonAsync($"{PATH}/events/{attendableEvent.Id}/attend", new { });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private void Authenticate(Guid userId)
    {
        var jwt = AuthHelper.GenerateJwt(userId.ToString());
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
