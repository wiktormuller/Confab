using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Repositories;

public interface ISubmissionRepository // Port
{
    Task<Submission> GetAsync(AggregateId id);
    Task AddAsync(Submission submission);
    Task UpdateAsync(Submission submission);
}
