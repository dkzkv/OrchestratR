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
        [NotNull] private readonly IServiceProvider _serviceProvider;

        public JobConsumer([NotNull] OrchestratedJob orchestratedJob, [NotNull] ILogger<JobConsumer> logger, [NotNull] JobManager jobManager,
            [NotNull] IServerPublisher serverPublisher, [NotNull] IServiceProvider serviceProvider)
        {
            _orchestratedJob = orchestratedJob ?? throw new ArgumentNullException(nameof(orchestratedJob));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
            _serverPublisher = serverPublisher ?? throw new ArgumentNullException(nameof(serverPublisher));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Consume(ConsumeContext<IStartJobMessage> context)
        {
            var jobCommand = context.Message;
            _logger.LogInformation($"Job message received: {jobCommand.JobName}");

            if (!_jobManager.IsExist(jobCommand.Id))
            {
                var cts = new CancellationTokenSource();
                var jobArgument = new JobArgument(jobCommand.JobName, jobCommand.Argument);
                await _jobManager.AddAndExecuteInfiniteJob(jobCommand.Id, async () =>
                {
                    await _orchestratedJob.Execute(jobArgument,
                        cts.Token,
                        HeartBeat(jobCommand.Id,cts.Token),
                        _serviceProvider);
                }, cts);
            }
            else
            {
                _logger.LogWarning($"Job with same key: {jobCommand.JobName}, already exist.");
            }

            _logger.LogInformation($"Job: {jobCommand.JobName} finished correctly.");
        }

        private  Func<Task> HeartBeat(Guid jobId, CancellationToken token)
        {
            return async () =>
            {
                await _serverPublisher.Publish(new JobHearBeatMessage(jobId, DateTimeOffset.Now), token);
            };
        }
    }
}