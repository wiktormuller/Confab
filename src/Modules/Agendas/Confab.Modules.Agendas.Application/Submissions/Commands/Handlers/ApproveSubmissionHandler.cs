using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Application.Submissions.Services;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

public sealed class ApproveSubmissionHandler : ICommandHandler<ApproveSubmission>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;

    public ApproveSubmissionHandler(ISubmissionRepository submissionRepository,
        IDomainEventDispatcher domainEventDispatcher,
        IEventMapper eventMapper,
        IMessageBroker messageBroker)
    {
        _submissionRepository = submissionRepository;
        _domainEventDispatcher = domainEventDispatcher;
        _eventMapper = eventMapper;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(ApproveSubmission command)
    {
        var submission = await _submissionRepository.GetAsync(command.Id);

        if (submission is null)
        {
            throw new SubmissionNotFoundException(command.Id);
        }

        submission.Approve();

        await _submissionRepository.UpdateAsync(submission);
        await _domainEventDispatcher.DispatchAsync(submission.Events.ToArray());

        var integrationEvents = _eventMapper.MapAll(submission.Events);
        await _messageBroker.PublishAsync(integrationEvents.ToArray());
    }
}
