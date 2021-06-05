using System;
using MediatR;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class MarkAsDeletedServerCommand : IRequest, ITransactionalCommand
    {
        public MarkAsDeletedServerCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}