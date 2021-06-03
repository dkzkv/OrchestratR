using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Domain;

namespace OrchestratR.ServerManager.Consumers
{
    [UsedImplicitly]
    public class JobStatusConsumer :  IConsumer<IJobStatusMessage>
    {
        private readonly JobOrchestrationService _jobOrchestrationService;

        public JobStatusConsumer([NotNull] JobOrchestrationService jobOrchestrationService)
        {
            _jobOrchestrationService = jobOrchestrationService ?? throw new ArgumentNullException(nameof(jobOrchestrationService));
        }

        public async Task Consume(ConsumeContext<IJobStatusMessage> context)
        {
            var jobMessage = context.Message;
            await _jobOrchestrationService.Update(jobMessage.Id, jobMessage.OrchestratedJobStatus, jobMessage.ServerId);
        }
    }
}