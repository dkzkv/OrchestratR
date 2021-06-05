using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using MediatR;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Domain.Commands;

namespace OrchestratR.ServerManager.Consumers
{
    [UsedImplicitly]
    public class JobStatusConsumer :  IConsumer<IJobStatusMessage>
    {
        private readonly IMediator _mediator;

        public JobStatusConsumer([NotNull] IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Consume(ConsumeContext<IJobStatusMessage> context)
        {
            var jobMessage = context.Message;
            await _mediator.Send(new UpdateJobCommand(jobMessage.Id, jobMessage.OrchestratedJobStatus, jobMessage.ServerId));
        }
    }
}