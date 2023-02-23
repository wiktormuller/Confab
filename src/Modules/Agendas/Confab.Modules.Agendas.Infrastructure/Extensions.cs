using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Modules.Agendas.Infrastructure.EF;
using Confab.Modules.Agendas.Infrastructure.EF.Repositories;
using Confab.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Agendas.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddScoped<ISpeakerRepository, SpeakerRepository>()
            .AddScoped<ISubmissionRepository, SubmissionRepository>()
            .AddPostgres<AgendasDbContext>()
            .AddScoped<IAgendaItemsRepository, AgendaItemsRepository>()
            .AddScoped<IAgendaTracksRepository, AgendaTracksRepository>();
}
