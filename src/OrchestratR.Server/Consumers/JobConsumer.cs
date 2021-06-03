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
    public class JobConsumer : IConsumer<IStartJobMessage>
    {
        private readonly OrchestratedJob _orchestratedJob;
        private readonly ILogger<JobConsumer> _logger;
        private readonly JobManager _jobManager;

        public JobConsumer(OrchestratedJob orchestratedJob, JobManager jobManager, ILogger<JobConsumer> logger)
        {
            _orchestratedJob = orchestratedJob ?? throw new ArgumentNullException(nameof(orchestratedJob));
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<IStartJobMessage> context)
        {
            var jobCommand = context.Message;
            _logger.LogInformation($"Job message received: {jobCommand.JobName}");

            if (!_jobManager.IsExist(jobCommand.Id))
            {
                var cts = new CancellationTokenSource();
                await _jobManager.AddAndExecuteInfiniteJob(jobCommand.Id, async () =>
                {
                    await _orchestratedJob.Execute(jobCommand.JobName,jobCommand.Argument,cts.Token);
                }, cts);
            }
            else
            {
                _logger.LogWarning($"Job with same key: {jobCommand.JobName}, already exist.");
            }
            _logger.LogInformation($"Job: {jobCommand.JobName} finished correctly.");
        }
    }
}