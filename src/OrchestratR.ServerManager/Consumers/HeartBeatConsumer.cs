using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Domain.Commands;

namespace OrchestratR.ServerManager.Consumers
{
    [UsedImplicitly]
    public class HeartBeatConsumer : IConsumer<IJobHearBeatMessage>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HeartBeatConsumer> _logger;

        public HeartBeatConsumer([NotNull] IMediator mediator, [NotNull] ILogger<HeartBeatConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<IJobHearBeatMessage> context)
        {
            var heartBeat = context.Message;
            try
            {
                await _mediator.Send(new UpdateHeartBeatCommand(heartBeat.JobId, heartBeat.HeartBeatTime));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,"HeartBeat didn't saved.");
            }
        }
    }
}