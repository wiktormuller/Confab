using Chronicle;
using Confab.Modules.Saga.Messages;
using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Modules.Saga.InviteSpeaker;

internal class InviteSpeakerSaga : Saga<SagaData>,
    ISagaStartAction<SignedUp>,
    ISagaAction<SpeakerCreated>,
    ISagaAction<SignedIn>
{
    private readonly IModuleClient _moduleClient;
    private readonly IMessageBroker _messageBroker;

    public InviteSpeakerSaga(IModuleClient moduleClient, 
        IMessageBroker messageBroker)
    {
        _moduleClient = moduleClient;
        _messageBroker = messageBroker;
    }

    public override SagaId ResolveId(object message, ISagaContext context)
        => message switch
        {
            SignedUp m => m.UserId.ToString(),
            SignedIn m => m.UserId.ToString(),
            SpeakerCreated m => m.Id.ToString(),
            _ => base.ResolveId(message, context)
        };

    public async Task HandleAsync(SignedUp message, ISagaContext context)
    {
        var (userId, email) = message;

        if (Data.InvitedSpeakers.TryGetValue(email, out var fullName))
        {
            Data.Email = email;
            Data.FullName = fullName;

            await _moduleClient.SendAsync("speakers/create", new
            {
                Id = userId,
                Email = email,
                FullName = fullName,
                Biio = "Lorem Ipsum"
            });

            return;
        }

        await CompleteAsync();
    }

    public Task HandleAsync(SpeakerCreated message, ISagaContext context)
    {
        Data.SpeakerCreated = true;
        return Task.CompletedTask;
    }

    public async Task HandleAsync(SignedIn message, ISagaContext context)
    {
        if (Data.SpeakerCreated)
        {
            await _messageBroker.PublishAsync(new SendWelcomeMessage(Data.Email, Data.FullName));
            await CompleteAsync();
        }
    }

    public Task CompensateAsync(SpeakerCreated message, ISagaContext context)
        => Task.CompletedTask;

    public Task CompensateAsync(SignedUp message, ISagaContext context)
        => Task.CompletedTask;

    public Task CompensateAsync(SignedIn message, ISagaContext context)
        => Task.CompletedTask;
}
