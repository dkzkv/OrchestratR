using System;

namespace OrchestratR.Core.Messages
{
    public interface IMessage
    {
        Guid CorrelationId { get; }
    }
}