using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Repositories
{
    public class JobRepository : BaseRepository, IJobRepository
    {
        public JobRepository(OrchestratorDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<OrchestratedJob> GetAsync(Guid id, CancellationToken token = default)
        {
            var result = await Context.OrchestratedJobs
                .Include(o => o.Server)
                .FirstOrDefaultAsync(o => o.Id.Equals(id), token);
            return Mapper.Map<OrchestratedJob>(result);
        }

        public async Task<OrchestratedJob> GetActiveAsync(string name, CancellationToken token = default)
        {
            var result = await Context.OrchestratedJobs
                .Include(o => o.Server)
                .FirstOrDefaultAsync(o => o.Name.Equals(name) && o.Status != JobLifecycleStatus.Deleted, token);
            return Mapper.Map<OrchestratedJob>(result);
        }

        public async Task<Guid> CreateAsync(OrchestratedJob orchestratedJob, CancellationToken token = default)
        {
            var existedOrchestrator = await GetAsync(orchestratedJob.Id, token);
            if (existedOrchestrator != null)
                throw new InvalidOperationException("Job with same id already exist!");

            var job = Mapper.Map<Entities.OrchestratedJob>(orchestratedJob);
            await Context.OrchestratedJobs.AddAsync(job, token);
            await Context.SaveChangesAsync(token);
            return job.Id;
        }

        public async Task UpdateAsync(OrchestratedJob orchestratedJob, CancellationToken token = default)
        {
            if (orchestratedJob is null)
                throw new ArgumentNullException(nameof(orchestratedJob));

            var existedJob = await Context.OrchestratedJobs
                .FirstOrDefaultAsync(o => o.Id.Equals(orchestratedJob.Id), token);
            if (existedJob == null)
                throw new InvalidOperationException("Job with same id not exist!, Can't update");

            if (existedJob.ServerId != orchestratedJob.Server.Id)
            {
                existedJob.ServerId = orchestratedJob.Server.Id;
                existedJob.Server = Mapper.Map<Entities.Server>(orchestratedJob.Server);
            }

            existedJob.Status = orchestratedJob.Status;
            existedJob.ModifyAt = orchestratedJob.ModifyAt;

            Context.OrchestratedJobs.Update(existedJob);
            await Context.SaveChangesAsync(token);
        }
    }
}