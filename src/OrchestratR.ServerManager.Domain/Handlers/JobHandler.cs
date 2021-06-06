using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using OrchestratR.Core;
using OrchestratR.Core.Publishers;
using OrchestratR.ServerManager.Domain.Commands;
using OrchestratR.ServerManager.Domain.Common.Exceptions;
using OrchestratR.ServerManager.Domain.Common.Messages;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Handlers
{
    public class JobHandler : IRequestHandler<CreateJobCommand, Guid>,
        IRequestHandler<UpdateJobCommand>,
        IRequestHandler<MarkAsDeletedJobCommand>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IServerManagerPublisher _serverManagerPublisher;
        private readonly IServerRepository _serverRepository;

        public JobHandler([NotNull] IJobRepository jobRepository,
            [NotNull] IServerManagerPublisher serverManagerPublisher,
            [NotNull] IServerRepository serverRepository)
        {
            _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            _serverManagerPublisher = serverManagerPublisher ?? throw new ArgumentNullException(nameof(serverManagerPublisher));
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        }

        public async Task<Guid> Handle(CreateJobCommand request, CancellationToken token)
        {
            var existedJob = await _jobRepository.GetActiveAsync(request.Name, token);
            if (existedJob is not null)
                throw new ExistedJobException(request.Name);
            var orchestratedJob = new OrchestratedJob(request.Name, request.Argument);
            var id = await _jobRepository.CreateAsync(orchestratedJob, token);
            await _serverManagerPublisher.Send(new StartJobMessage(id, request.Name, request.Argument, orchestratedJob.CreatedAt), token);
            return id;
        }

        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken token)
        {
            var existedJob = await _jobRepository.GetAsync(request.Id, token);
            if (existedJob is null)
                throw new InvalidOperationException($"Can't update Job; Id: {request.Id}. Not exist.");

            var server = await _serverRepository.GetAsync(request.ServerId, token);
            if (server is null)
                throw new InvalidOperationException($"Can't update Job; Id: {request.Id} for not existed server: {request.ServerId}.");

            var updatedJob = existedJob.SetServer(server)
                .UpdateStatus(request.Status == OrchestratedJobStatus.Activated ? JobLifecycleStatus.Processing : JobLifecycleStatus.Deleted);

            await _jobRepository.UpdateAsync(updatedJob, token);
            return Unit.Value;
        }

        public async Task<Unit> Handle(MarkAsDeletedJobCommand request, CancellationToken token)
        {
            var existedJob = await _jobRepository.GetAsync(request.Id, token);
            
            if (existedJob is null)
                throw new NotExistedJobException(request.Id);
            
            if (existedJob.Status != JobLifecycleStatus.Deleted)
            {
                await _jobRepository.UpdateAsync(existedJob.UpdateStatus(JobLifecycleStatus.OnDeleting), token);
                await _serverManagerPublisher.Publish(new StopJobMessage(request.Id), token);
            }
            return Unit.Value;
        }
    }
}