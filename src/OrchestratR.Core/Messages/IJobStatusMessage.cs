using System;

namespace OrchestratR.Core.Messages
{
    public interface IJobStatusMessage :  IMessage
    {
        Guid Id { get; }
        Guid ServerId { get; }
        OrchestratedJobStatus OrchestratedJobStatus { get; }
        DateTimeOffset UpdatedAt { get; }
    }
}