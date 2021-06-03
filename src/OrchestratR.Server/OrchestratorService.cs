using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchestratR.Server.Common;

namespace OrchestratR.Server
{
    internal class OrchestratorService : IHostedService
    {
        private readonly JobManager _jobManager;
        private readonly IBusControl _busControl;
        private readonly ILogger<OrchestratorService> _logger;

        public OrchestratorService(JobManager jobManager, IBusControl busControl, ILogger<OrchestratorService> logger)
        {
            _jobManager = jobManager;
            _busControl = busControl;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration initiation started.");
            
            await _busControl.StartAsync(token);
            _logger.LogDebug("Orchestrator connected to message broker.");
            
            await _jobManager.ActivateManager();
            _logger.LogDebug("Job manager activated.");
        }

        public async Task StopAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration disposing started.");
            
            await _jobManager.DiActivateManager(token, false);
            _logger.LogDebug("Orchestration manager di-activated.");
            
            await _busControl.StopAsync(token);
            _logger.LogDebug("Orchestrator message broker disconnected.");

            await _jobManager.DisposeAsync();
            _logger.LogDebug("Orchestration manager disposed.");
        }
    }
}