using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

public class EmptySumbissionTitleException : ConfabException
{
    public Guid SubmissionId { get; }

    public EmptySumbissionTitleException(Guid submissionId) : base($"Submission with ID: '{submissionId}' defines empty tiitle.")
    {
        SubmissionId = submissionId;
    }
}
