using System;
using OrchestratR.Core.Messages;

namespace OrchestratR.Server.Messaging
{
    internal class JobHearBeatMessage : BaseMessage, IJobHearBeatMessage
    {
        public JobHearBeatMessage(Guid jobId, DateTimeOffset heartBeatTime)
        {
            JobId = jobId;
            HeartBeatTime = heartBeatTime;
        }
        public Guid JobId { get; }
        public DateTimeOffset HeartBeatTime { get; }
    }
}