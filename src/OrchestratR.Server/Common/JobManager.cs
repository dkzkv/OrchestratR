using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OrchestratR.Core;
using OrchestratR.Core.Publishers;
using OrchestratR.Server.Messaging;
using OrchestratR.Server.Options;

namespace OrchestratR.Server.Common
{
    public class JobManager : IAsyncDisposable
    {
        private readonly OrchestratedServerOptions _orchestratedServerOptions;
        private readonly IServerPublisher _serverPublisher;
        private readonly ILogger<JobManager> _logger;

        private Dictionary<Guid, CancellationTokenSource> _jobCancelManager;
        private Guid? _serverIdentifier;

        public JobManager([NotNull] IServerPublisher serverPublisher,
            OrchestratedServerOptions orchestratedServerOptions,
            ILogger<JobManager> logger)
        {
            _serverPublisher = serverPublisher ?? throw new ArgumentNullException(nameof(serverPublisher));
            _orchestratedServerOptions = orchestratedServerOptions ?? throw new ArgumentNullException(nameof(orchestratedServerOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ActivateManager(CancellationToken token)
        {
            _serverIdentifier = Guid.NewGuid();
            _jobCancelManager = new Dictionary<Guid, CancellationTokenSource>();
            var message = new ServerCreatedMessage(_serverIdentifier.Value,
                _orchestratedServerOptions.OrchestratorName,
                _orchestratedServerOptions.MaxWorkersCount,
                DateTimeOffset.Now);
            
            await _serverPublisher.Send(message,token);
        }

        public async Task DiActivateManager(CancellationToken token, bool diActivateWithCancellation = true)
        {
            if (_serverIdentifier.HasValue)
            {
                await _serverPublisher.Send(new ServerDeletedMessage(_serverIdentifier.Value, DateTimeOffset.Now), token);
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
                await NotifyDistributorActivated(jobId,tokenSource.Token);
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

        public async Task CancelJob(Guid jobId, CancellationToken token)
        {
            CheckIsManagerActivated();
            if (_jobCancelManager.ContainsKey(jobId))
            {
                _jobCancelManager[jobId].Cancel();
                _jobCancelManager.Remove(jobId);
                await NotifyDistributorDiActivated(jobId,token);
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

        private async Task NotifyDistributorActivated(Guid jobId, CancellationToken token)
        {
            if (_serverIdentifier.HasValue)
                await _serverPublisher.Send(
                    new JobStatusMessage(jobId, _serverIdentifier.Value, OrchestratedJobStatus.Activated, DateTimeOffset.Now),token);
        }

        private async Task NotifyDistributorDiActivated(Guid jobIde,CancellationToken token = default)
        {
            if (_serverIdentifier.HasValue)
                await _serverPublisher.Send(
                    new JobStatusMessage(jobIde, _serverIdentifier.Value, OrchestratedJobStatus.DiActivated, DateTimeOffset.Now),token);
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