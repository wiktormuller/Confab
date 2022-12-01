using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exceptions;

public class CannotDeleteConferenceException : ConfabException
{
    public Guid Id { get; }
    public CannotDeleteConferenceException(Guid id) : base($"Conference with Id: '{id}' cannot be deleted.")
    {
        Id = id;
    }
}