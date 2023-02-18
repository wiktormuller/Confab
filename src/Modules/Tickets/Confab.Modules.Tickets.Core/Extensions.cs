using System.Runtime.CompilerServices;
using Confab.Modules.Tickets.Core.DAL;
using Confab.Modules.Tickets.Core.Repositories;
using Confab.Modules.Tickets.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Confab.Shared.Infrastructure.Postgres;

[assembly: InternalsVisibleTo("Confab.Modules.Tickets.Api")]
namespace Confab.Modules.Tickets.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services
            .AddScoped<ITicketService, TicketService>()
            .AddScoped<ITicketSaleService, TicketSaleService>()
            .AddScoped<IConferenceRepository, ConferenceRepository>()
            .AddScoped<ITicketRepository, TicketRepository>()
            .AddScoped<ITicketSaleRepository, TicketSaleRepository>()
            .AddSingleton<ITicketGenerator, TicketGenerator>()
            .AddPostgres<TicketsDbContext>();
    }
}