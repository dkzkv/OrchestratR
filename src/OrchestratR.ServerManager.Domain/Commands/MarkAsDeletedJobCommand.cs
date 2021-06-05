using System;
using MediatR;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class MarkAsDeletedJobCommand : IRequest, ITransactionalCommand
    {
        public MarkAsDeletedJobCommand(Guid id)
        {
            if(id.Equals(Guid.Empty))
                throw new AggregateException($"{nameof(id)} empty guid");
            Id = id;
        }

        public Guid Id { get; }
    }
}