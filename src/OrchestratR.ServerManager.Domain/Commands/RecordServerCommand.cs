using System;
using JetBrains.Annotations;
using MediatR;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class RecordServerCommand : IRequest, ITransactionalCommand
    {
        public RecordServerCommand([NotNull] Server server)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));
        }
        public Server Server { get; }
    }
}