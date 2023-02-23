using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Consts;
using Confab.Modules.Agendas.Domain.Submissions.Events;
using Confab.Shared.Abstractions.Kernel;

namespace Confab.Modules.Agendas.Domain.Agendas.Events.Handlers
{
    internal sealed class SubmissionApprovedHandler : IDomainEventHandler<SubmissionStatusChanged>
    {
        private readonly IAgendaItemsRepository _agendaItemsRepository;

        public SubmissionApprovedHandler(IAgendaItemsRepository agendaItemsRepository)
        {
            _agendaItemsRepository = agendaItemsRepository;
        }

        // When submission was approved then we can create agendaItem (e.g. presentation, prelection etc.)
        // Also this action can be handled without generating events, for example by command handler, if the speaker is someone important
        // We can ommit the flow of submission and for example create agendaItem directly from admin panel.
        public async Task HandleAsync(SubmissionStatusChanged @event)
        {
            if (@event.Status is SubmissionStatus.Rejected)
            {
                return;
            }

            var submission = @event.Submission; // Domain event contains whole aggregate of submission
            var agendaItem = await _agendaItemsRepository.GetAsync(submission.Id);

            if (agendaItem is not null)
            {
                return;
            }

            agendaItem = AgendaItem.Create(submission.Id, submission.ConferenceId, submission.Title,
                submission.Description, submission.Level, submission.Tags, submission.Speakers.ToList());

            await _agendaItemsRepository.AddAsync(agendaItem);
        }
    }
}
