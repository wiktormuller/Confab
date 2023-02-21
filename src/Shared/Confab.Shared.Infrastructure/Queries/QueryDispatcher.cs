using Confab.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Queries;

internal sealed class QueryDispatcher : IQueryDispatcher // Thanks to dispatcher we don't have to register every handler manually.
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = _serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType); // One and only one command handler for command is required

        return await (Task<TResult>)handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
            ?.Invoke(handler, new[] { query });
    }
}
