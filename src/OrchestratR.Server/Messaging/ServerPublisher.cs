using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using OrchestratR.Core.Messages;
using OrchestratR.Core.Publishers;

namespace OrchestratR.Server.Messaging
{
    public class ServerPublisher : IServerPublisher
    {
        [NotNull] private readonly IBus _bus;
        [NotNull] private readonly IPublishEndpoint _publishEndpoint;

        public ServerPublisher([NotNull] IBus bus, [NotNull] IPublishEndpoint publishEndpoint)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Send(IServerCreatedMessage message, CancellationToken token)
        {
            await _bus.Send(message, token);
        }

        public async Task Send(IJobStatusMessage message, CancellationToken token)
        {
            await _bus.Send(message, token);
        }

        public async Task Publish(IJobHearBeatMessage message, CancellationToken token)
        {
            await _publishEndpoint.Publish(message, token);
        }

        public async Task Send(IServerDeletedMessage message, CancellationToken token)
        {
            await _bus.Send(message, token);
        }
    }
}