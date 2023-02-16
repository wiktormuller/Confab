using System.Runtime.CompilerServices;
using Confab.Modules.Speakers.Core.DAL;
using Confab.Modules.Speakers.Core.Repositories;
using Confab.Modules.Speakers.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Confab.Shared.Infrastructure.Postgres;

[assembly: InternalsVisibleTo("Confab.Modules.Speakers.Api")] // Now, it's visible to Api project ONLY

namespace Confab.Modules.Speakers.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
            => services
                .AddScoped<ISpeakersService, SpeakersService>()
                .AddScoped<ISpeakersRepository, SpeakersRepository>()
                .AddPostgres<SpeakersDbContext>();
    }
}