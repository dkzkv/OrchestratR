using System;

namespace OrchestratR.Core.Messages
{
    public abstract class BaseMessage : IMessage
    {
        protected BaseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }
        public Guid CorrelationId { get; }
    }
}