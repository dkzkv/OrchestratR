using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchestratR.Server.Common;

namespace OrchestratR.Server
{
    internal class OrchestratorService : IHostedService
    {
        [NotNull] private readonly OrchestratrReceiveEndpointObserver _endpointObserver;
        [NotNull] private readonly JobManager _jobManager;
        [NotNull] private readonly IBusControl _busControl;
        [NotNull] private readonly ILogger<OrchestratorService> _logger;

        public OrchestratorService([NotNull] OrchestratrReceiveEndpointObserver endpointObserver, [NotNull] JobManager jobManager,
            [NotNull] IBusControl busControl, [NotNull] ILogger<OrchestratorService> logger)
        {
            _endpointObserver = endpointObserver ?? throw new ArgumentNullException(nameof(endpointObserver));
            _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration initiation started.");
            
            _busControl.ConnectReceiveEndpointObserver(_endpointObserver);
            await _busControl.StartAsync(token);
            
            _logger.LogDebug("Orchestrator connected to message broker.");
            
            await _jobManager.ActivateManager(token);
            _logger.LogDebug("Job manager activated.");
        }

        public async Task StopAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration disposing started.1");
            
            await _jobManager.DiActivateManager(token, false);
            _logger.LogDebug("Orchestration manager di-activated.");
            
            await _busControl.StopAsync(token);
            _logger.LogDebug("Orchestrator message broker disconnected.");
        }
    }
}