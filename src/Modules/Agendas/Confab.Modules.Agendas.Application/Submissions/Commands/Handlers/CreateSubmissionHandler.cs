using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Application.Submissions.Services;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel;
using Confab.Shared.Abstractions.Kernel.Types;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

public sealed class CreateSubmissionHandler : ICommandHandler<CreateSubmission>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ISpeakerRepository _speakerRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;

    public CreateSubmissionHandler(ISubmissionRepository submissionRepository, ISpeakerRepository speakerRepository,
        IDomainEventDispatcher domainEventDispatcher, IEventMapper eventMapper, IMessageBroker messageBroker)
    {
        _submissionRepository = submissionRepository;
        _speakerRepository = speakerRepository;
        _domainEventDispatcher = domainEventDispatcher;
        _eventMapper = eventMapper;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(CreateSubmission command)
    {
        var speakerIds = command.Speakers
            .Select(x => new AggregateId(x))
            .ToList();

        var speakers = await _speakerRepository.BrowseAsync(speakerIds);

        if (speakers.Count() != speakerIds.Count())
        {
            throw new InvalidSpeakersNumberException(command.Id);
        }

        var submission = Submission.Create(command.Id, command.ConferenceId, command.Title, command.Description,
            command.Level, command.Tags, speakers.ToList()); // This method can throw exception because use modifier methods in implementation

        await _submissionRepository.AddAsync(submission);
        await _domainEventDispatcher.DispatchAsync(submission.Events.ToArray());
        
        var integrationEvents = _eventMapper.MapAll(submission.Events);
        await _messageBroker.PublishAsync(integrationEvents.ToArray());
    }
}
