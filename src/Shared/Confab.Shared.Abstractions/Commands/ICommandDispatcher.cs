namespace Confab.Shared.Abstractions.Commands;

public interface ICommandDispatcher
{
    public Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
}
