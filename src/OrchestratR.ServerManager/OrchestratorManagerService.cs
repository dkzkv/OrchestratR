using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;
using OrchestratR.ServerManager.Persistence;

namespace OrchestratR.ServerManager
{
    public class OrchestratorManagerService: IHostedService
    {
        private readonly OrchestratorDbContext _context;
        private readonly ILogger<OrchestratorManagerService> _logger;
        private readonly IBusControl _busControl;
        private readonly IJobRepository _jobRepository;

        public OrchestratorManagerService([NotNull] OrchestratorDbContext context,
            [NotNull] ILogger<OrchestratorManagerService> logger, [NotNull] IBusControl busControl,
            IJobRepository jobRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));

            _jobRepository = jobRepository;
        }

        public async Task StartAsync(CancellationToken token)
        {
            _logger.LogDebug("Orchestration manager initiation started.");
            
            _logger.LogDebug("Migration started.");
            await _context.Database.MigrateAsync(token);
            _logger.LogDebug("Migration successfully finished.");

            /*var job = new OrchestratedJob("some", "some-arg");
            var id = await _jobRepository.CreateAsync(job);
            
            var savedJob = await _jobRepository.GetAsync(id);
            savedJob.SetServer(new Server(Guid.NewGuid(), "server-1", 100, DateTimeOffset.Now));
            await _jobRepository.UpdateAsync(savedJob);
            
            var afterUpdate = await _jobRepository.GetAsync(id);*/
            
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