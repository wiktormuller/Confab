using Confab.Shared.Abstractions.Kernel.Types;
using System.Runtime.Serialization;

namespace Confab.Modules.Agendas.Domain.Agendas.Entities
{
    [Serializable]
    internal class EmptySubmissionTitleException : Exception
    {
        private AggregateId id;

        public EmptySubmissionTitleException()
        {
        }

        public EmptySubmissionTitleException(AggregateId id)
        {
            this.id = id;
        }

        public EmptySubmissionTitleException(string? message) : base(message)
        {
        }

        public EmptySubmissionTitleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmptySubmissionTitleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}