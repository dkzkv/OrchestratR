using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrchestratR.Core;
using OrchestratR.Server.Messages;
using OrchestratR.Server.Options;

namespace OrchestratR.Server.Common
{
    public class JobManager : IAsyncDisposable
    {
        private readonly OrchestratedServerOptions _orchestratedServerOptions;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<JobManager> _logger;

        private Dictionary<Guid, CancellationTokenSource> _jobCancelManager;
        private Guid? _serverIdentifier;

        public JobManager(IPublishEndpoint publishEndpoint, OrchestratedServerOptions orchestratedServerOptions, ILogger<JobManager> logger)
        {
            _orchestratedServerOptions = orchestratedServerOptions ?? throw new ArgumentNullException(nameof(orchestratedServerOptions));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ActivateManager()
        {
            _serverIdentifier = Guid.NewGuid();
            _jobCancelManager = new Dictionary<Guid, CancellationTokenSource>();
            await _publishEndpoint.Publish(
                new ServerCreatedMessage(_serverIdentifier.Value,
                    _orchestratedServerOptions.OrchestratorName,
                    _orchestratedServerOptions.MaxWorkersCount,
                    DateTimeOffset.Now));
        }

        public async Task DiActivateManager(CancellationToken token, bool diActivateWithCancellation = true)
        {
            if (_serverIdentifier.HasValue)
            {
                await _publishEndpoint.Publish(new ServerDeletedMessage(_serverIdentifier.Value, DateTimeOffset.Now), token);
                if (diActivateWithCancellation)
                    await CancelAll(false);
            }
        }

        public async Task AddAndExecuteInfiniteJob(Guid jobId, Func<Task> func, CancellationTokenSource tokenSource)
        {
            CheckIsManagerActivated();
            if (!_jobCancelManager.ContainsKey(jobId))
            {
                _jobCancelManager.Add(jobId, tokenSource);
                await NotifyDistributorActivated(jobId);
                await ExecuteInfiniteJob(func, jobId, tokenSource.Token);
            }
            else
            {
                _jobCancelManager[jobId] = tokenSource;
            }
        }

        public bool IsExist(Guid jobId)
        {
            CheckIsManagerActivated();
            return _jobCancelManager.ContainsKey(jobId);
        }

        public async Task CancelJob(Guid jobId)
        {
            CheckIsManagerActivated();
            if (_jobCancelManager.ContainsKey(jobId))
            {
                _jobCancelManager[jobId].Cancel();
                _jobCancelManager.Remove(jobId);
                await NotifyDistributorDiActivated(jobId);
            }
        }

        public async Task CancelAll(bool isSilentCancellation = true)
        {
            CheckIsManagerActivated();
            foreach (var jobInfo in _jobCancelManager)
            {
                jobInfo.Value.Cancel();
                if (!isSilentCancellation)
                    await NotifyDistributorDiActivated(jobInfo.Key);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_serverIdentifier.HasValue)
            {
                await CancelAll();
            }

            _serverIdentifier = null;
            _jobCancelManager?.Clear();
        }

        private void CheckIsManagerActivated()
        {
            if (!_serverIdentifier.HasValue)
                throw new InvalidOperationException("Manager is not activated.");
        }

        private async Task NotifyDistributorActivated(Guid jobId)
        {
            if (_serverIdentifier.HasValue)
                await _publishEndpoint.Publish(
                    new JobStatusMessage(jobId, _serverIdentifier.Value, OrchestratedJobStatus.Activated, DateTimeOffset.Now));
        }

        private async Task NotifyDistributorDiActivated(Guid jobIde)
        {
            if (_serverIdentifier.HasValue)
                await _publishEndpoint.Publish(
                    new JobStatusMessage(jobIde, _serverIdentifier.Value, OrchestratedJobStatus.DiActivated, DateTimeOffset.Now));
        }

        private async Task ExecuteInfiniteJob(Func<Task> func, Guid id, CancellationToken token)
        {
            int errorCounter = 0;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await func.Invoke();
                }
                catch (TaskCanceledException)
                {
                    _logger.LogDebug($"Job: {id} finished with cancellation token correctly.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Job: {id} finished with error, error count: {++errorCounter}");
                }
            }
        }
    }
}