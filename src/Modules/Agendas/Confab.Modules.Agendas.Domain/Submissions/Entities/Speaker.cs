using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Entities;

public class Speaker : AggregateRoot
{
    public string FullName { get; init; }

    private ICollection<Submission> _submissions; // Require by EF Core
    public IEnumerable<Submission> Submissions => _submissions; // Require by EF Core

    public Speaker(AggregateId id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    // Factory method
    public static Speaker Create(Guid id, string fullName)
        => new Speaker(id, fullName);
}
