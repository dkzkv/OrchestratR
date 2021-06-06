using System;

namespace OrchestratR.Core.Messages
{
    public interface IJobHearBeatMessage : IMessage
    {
        Guid JobId { get; }
        DateTimeOffset HeartBeatTime { get; }
    }
}