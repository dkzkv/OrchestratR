using System;
using System.Threading;
using System.Threading.Tasks;
using OrchestratR.ServerManager.Domain.Models;


namespace OrchestratR.ServerManager.Domain.Interfaces
{
    public interface IJobRepository
    {
        
        Task<OrchestratedJob> GetAsync(Guid i, CancellationToken token = default);
        Task<OrchestratedJob> GetActiveAsync(string name, CancellationToken token = default);
        Task<Guid> CreateAsync(OrchestratedJob orchestratedJob, CancellationToken token = default);
        Task UpdateAsync(OrchestratedJob orchestratedJob, CancellationToken token = default);
    }
}