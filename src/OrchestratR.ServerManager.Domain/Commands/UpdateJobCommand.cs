using System;
using MediatR;
using OrchestratR.Core;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class UpdateJobCommand : IRequest, ITransactionalCommand
    {
        public UpdateJobCommand(Guid id, OrchestratedJobStatus status, Guid serverId)
        {
            if(id.Equals(Guid.Empty))
                throw new AggregateException($"{nameof(id)} empty guid");
            
            if(id.Equals(Guid.Empty))
                throw new AggregateException($"{nameof(serverId)} empty guid");
            
            Id = id;
            Status = status;
            ServerId = serverId;
        }
        
        public Guid Id { get; }
        public OrchestratedJobStatus Status { get; }
        public Guid ServerId { get; }
    }
}