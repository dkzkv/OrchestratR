using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using OrchestratR.Core;
using OrchestratR.ServerManager.Domain.Common.Messages;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain
{
    public class JobOrchestrationService : IJobOrchestrationClient
    {
        private readonly IJobRepository _jobRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IServerRepository _serverRepository;

        public JobOrchestrationService([NotNull] IJobRepository jobRepository,
            [NotNull] IPublishEndpoint publishEndpoint,
            [NotNull] IServerRepository serverRepository)
        {
            _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        }

        public async Task<Guid> Create(string name, string argument, CancellationToken token = default)
        {
            var existedJob = await _jobRepository.GetActiveAsync(name,token);
            if (existedJob is not null)
                throw new InvalidOperationException($"Job with name: {name}, already exists");
            var orchestratedJob = new OrchestratedJob(name, argument);
            var id = await _jobRepository.CreateAsync(orchestratedJob, token);
            await _publishEndpoint.Publish(new StartJobMessage(id,name, argument, orchestratedJob.CreatedAt), token);
            return id;
        }
        
        public async Task Update(Guid id, OrchestratedJobStatus status, Guid serverId, CancellationToken token = default)
        {
            var existedJob = await _jobRepository.GetAsync(id,token);
            if (existedJob is null)
                throw new InvalidOperationException($"Can't update Job; Id: {id}. Not exist.");

            var server = await _serverRepository.GetAsync(serverId, token);
            if (server is null)
                throw new InvalidOperationException($"Can't update Job; Id: {id} for not existed server: {serverId}.");

            var updatedJob = existedJob.SetServer(server)
                .UpdateStatus(status == OrchestratedJobStatus.Activated ? JobLifecycleStatus.Processing : JobLifecycleStatus.Deleted);

            await _jobRepository.UpdateAsync(updatedJob, token);
        }

        public async Task MarkOnDeleting(Guid id, CancellationToken token = default)
        {
            var existedJob = await _jobRepository.GetAsync(id, token);
            if (existedJob != null && existedJob.Status != JobLifecycleStatus.Deleted)
            {
                await _jobRepository.UpdateAsync(existedJob.UpdateStatus(JobLifecycleStatus.OnDeleting), token);
                await _publishEndpoint.Publish(new StopJobMessage(id), token);
            }
        }
    }
}