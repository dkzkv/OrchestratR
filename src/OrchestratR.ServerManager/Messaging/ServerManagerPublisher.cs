using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using OrchestratR.Core.Messages;
using OrchestratR.Core.Publishers;

namespace OrchestratR.ServerManager.Messaging
{
    public class ServerManagerPublisher : IServerManagerPublisher
    {
        private readonly IBus _bus;
        private readonly IPublishEndpoint _publishEndpoint;

        public ServerManagerPublisher([NotNull] IBus bus, [NotNull] IPublishEndpoint publishEndpoint)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Send(IStartJobMessage message, CancellationToken token = default)
        {
            await _bus.Send(message, token);
        }

        public async Task Publish(IStopJobMessage message, CancellationToken token = default)
        {
            await _publishEndpoint.Publish(message,token);
        }
    }
}