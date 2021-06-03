using System;
using OrchestratR.Core.Messages;

namespace OrchestratR.ServerManager.Domain.Common.Messages
{
    internal class StopJobMessage : BaseMessage, IStopJobMessage
    {
        public StopJobMessage(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}