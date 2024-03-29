using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrchestratR.Core.Messages;
using OrchestratR.Server.Common;

namespace OrchestratR.Server.Consumers
{
    [UsedImplicitly]
    public class JobCancellationConsumer : IConsumer<IStopJobMessage>
    {
        private readonly JobManager _jobManager;
        private readonly ILogger<JobCancellationConsumer> _logger;
        
        public JobCancellationConsumer([NotNull] JobManager jobManager, [NotNull] ILogger<JobCancellationConsumer> logger)
        {
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Consume(ConsumeContext<IStopJobMessage> context)
        {
            var jobId = context.Message.Id;
            if (_jobManager.IsExist(jobId))
            {
                _logger.LogInformation($"Job cancellation handled, Job with Id: {jobId} exist, cancellation started.");
                await _jobManager.CancelJob(jobId, CancellationToken.None);
            }
            else
            {
                _logger.LogInformation($"Job cancellation handled, Job with Id: {jobId} not exist");
            }
        }
    }
}