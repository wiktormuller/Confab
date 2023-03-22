namespace Confab.Modules.Saga.InviteSpeaker
{
    internal class SagaData
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool SpeakerCreated { get; set; }

        public readonly Dictionary<string, string> InvitedSpeakers = new()
        {
            ["testspeaker1@confab.io"] = "John Smith",
            ["testspeaker2@confab.io"] = "Mark Sim"
        };
    }
}
