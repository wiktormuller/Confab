using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

public class EmptySumbissionDescriptionException : ConfabException
{
    public Guid SubmissionId { get; }

    public EmptySumbissionDescriptionException(Guid submissionId) 
        : base($"Submission with ID: '{submissionId}' defines empty description.")
    {
        SubmissionId = submissionId;
    }
}
