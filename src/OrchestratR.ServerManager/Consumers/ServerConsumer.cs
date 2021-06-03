using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Domain;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Consumers
{
    [UsedImplicitly]
    public class ServerConsumer : IConsumer<IServerCreatedMessage>, IConsumer<IServerDeletedMessage>
    {
        private readonly ServerService _serverService;

        public ServerConsumer([NotNull] ServerService serverService)
        {
            _serverService = serverService ?? throw new ArgumentNullException(nameof(serverService));
        }

        public async Task Consume(ConsumeContext<IServerCreatedMessage> context)
        {
            var server = context.Message;
            await _serverService.Record(new Server(server.Id, server.Name, server.MaxWorkersCount, server.CreatedAt));
        }

        public async Task Consume(ConsumeContext<IServerDeletedMessage> context)
        {
            var server = context.Message;
            await _serverService.MarkAsDeleted(server.Id);
        }
    }
}