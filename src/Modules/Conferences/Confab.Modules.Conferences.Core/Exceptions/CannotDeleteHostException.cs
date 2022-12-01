using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exceptions;

public class CannotDeleteHostException : ConfabException
{
    public Guid Id { get; }

    public CannotDeleteHostException(Guid id) : base($"Host with Id: '{id}' cannot be deleted.")
    {
        Id = id;
    }
}