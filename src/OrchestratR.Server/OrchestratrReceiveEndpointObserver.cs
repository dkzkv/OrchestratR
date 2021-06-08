using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrchestratR.Server.Common;

namespace OrchestratR.Server
{
    internal class OrchestratrReceiveEndpointObserver : IReceiveEndpointObserver
    {
        [NotNull] private readonly ILogger<OrchestratrReceiveEndpointObserver> _logger;
        [NotNull] private readonly IOrchestratrObserverFaultRule _rule;
        [NotNull] private readonly JobManager _jobManager;

        public OrchestratrReceiveEndpointObserver([NotNull] ILogger<OrchestratrReceiveEndpointObserver> logger,
            IOrchestratrObserverFaultRule rule,
            [NotNull] JobManager jobManager)
        {
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
        }

        public Task Ready(ReceiveEndpointReady ready)
        {
            return Task.CompletedTask;
        }

        public Task Stopping(ReceiveEndpointStopping stopping)
        {
            return Task.CompletedTask;
        }

        public Task Completed(ReceiveEndpointCompleted completed)
        {
            return Task.CompletedTask;
        }

        public async Task Faulted(ReceiveEndpointFaulted faulted)
        {
            if (faulted.Exception.GetType() == _rule.ErrorType)
            {
                if (faulted.InputAddress.AbsolutePath == _rule.AbsolutePath)
                {
                    _logger.LogWarning("Transport connection problems, emergency cancellation started.");
                    await _jobManager.CancelAll();
                }
            }
        }
    }
}