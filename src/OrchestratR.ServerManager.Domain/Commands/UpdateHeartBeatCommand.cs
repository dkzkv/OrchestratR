using System;
using MediatR;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class UpdateHeartBeatCommand : IRequest
    {
        public UpdateHeartBeatCommand(Guid jobId, DateTimeOffset heartBeatTime)
        {
            JobId = jobId;
            HeartBeatTime = heartBeatTime;
        }

        public Guid JobId { get; }
        public DateTimeOffset HeartBeatTime { get; }
    }
}