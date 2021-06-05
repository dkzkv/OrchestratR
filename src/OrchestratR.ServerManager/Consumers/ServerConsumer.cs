using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using MediatR;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Domain.Commands;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Consumers
{
    [UsedImplicitly]
    public class ServerConsumer : IConsumer<IServerCreatedMessage>, IConsumer<IServerDeletedMessage>
    {
        private readonly IMediator _mediator;

        public ServerConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IServerCreatedMessage> context)
        {
            var server = context.Message;
            await _mediator.Send(new RecordServerCommand(new Server(server.Id, server.Name, server.MaxWorkersCount, server.CreatedAt)));
        }

        public async Task Consume(ConsumeContext<IServerDeletedMessage> context)
        {
            var server = context.Message;
            await _mediator.Send(new MarkAsDeletedServerCommand(server.Id));
        }
    }
}