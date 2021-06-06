using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrchestratR.Core.Messages;
using OrchestratR.Core.Publishers;
using OrchestratR.Server.Common;
using OrchestratR.Server.Messaging;

namespace OrchestratR.Server.Consumers
{
    [UsedImplicitly]
    public class JobConsumer : IConsumer<IStartJobMessage>
    {
        [NotNull] private readonly OrchestratedJob _orchestratedJob;
        [NotNull] private readonly ILogger<JobConsumer> _logger;
        [NotNull] private readonly JobManager _jobManager;
        [NotNull] private readonly IServerPublisher _serverPublisher;

        public JobConsumer([NotNull] OrchestratedJob orchestratedJob, [NotNull] ILogger<JobConsumer> logger, [NotNull] JobManager jobManager,
            [NotNull] IServerPublisher serverPublisher)
        {
            _orchestratedJob = orchestratedJob ?? throw new ArgumentNullException(nameof(orchestratedJob));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
            _serverPublisher = serverPublisher ?? throw new ArgumentNullException(nameof(serverPublisher));
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
                    var jobArgument = new JobArgument(jobCommand.JobName, jobCommand.Argument);
                    await _orchestratedJob.Execute(jobArgument, cts.Token,
                        async () =>
                        {
                            await _serverPublisher.Publish(new JobHearBeatMessage(jobCommand.Id, DateTimeOffset.Now), cts.Token);
                        });
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