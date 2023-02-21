namespace Confab.Shared.Abstractions.Queries;

public interface IQueryDispatcher
{
    // We could make both of these parameters (TResult, TQuery) as generic,
    // but then we should specify two of those parameters in real query class
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}
