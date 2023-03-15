namespace Confab.Shared.Infrastructure.Postgres;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}
