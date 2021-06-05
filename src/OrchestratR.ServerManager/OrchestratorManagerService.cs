using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchestratR.ServerManager.Persistence;

namespace OrchestratR.ServerManager
{
    internal class OrchestratorManagerService : IHostedService, IOrchestratorManagerService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrchestratorManagerService> _logger;
        private readonly IBusControl _busControl;

        public OrchestratorManagerService(IServiceScopeFactory serviceScopeFactory,
            [NotNull] ILogger<OrchestratorManagerService> logger, [NotNull] IBusControl busControl)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        }

        public async Task StartAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration manager initiation started.");

            _logger.LogDebug("Migration started.");
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<OrchestratorDbContext>();
                if (ctx is null)
                    throw new InvalidOperationException("Can't provide db context.");
                
                await ctx.Database.MigrateAsync(token);
                _logger.LogDebug("Migration successfully finished.");
            }

            await _busControl.StartAsync(token);
            _logger.LogDebug("Orchestrator connected to message broker.");
        }

        public async Task StopAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration manager disposing started.");

            await _busControl.StopAsync(token);
            _logger.LogDebug("Orchestrator manager message broker disconnected.");
        }
    }
}