using System;
using OrchestratR.Core;
using OrchestratR.Core.Messages;

namespace OrchestratR.Server.Messaging
{
    internal class JobStatusMessage :  BaseMessage, IJobStatusMessage
    {
        public JobStatusMessage(Guid id, Guid serverId, OrchestratedJobStatus orchestratedJobStatus, DateTimeOffset updatedAt)
        {
            Id = id;
            ServerId = serverId;
            OrchestratedJobStatus = orchestratedJobStatus;
            UpdatedAt = updatedAt;
        }

        public Guid Id { get; }
        public Guid ServerId { get; }
        public OrchestratedJobStatus OrchestratedJobStatus { get; }
        public DateTimeOffset UpdatedAt { get; }
    }
}