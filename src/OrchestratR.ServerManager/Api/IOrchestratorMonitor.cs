using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Api
{
    public interface IAdminOrchestratorMonitor : IOrchestratedJobMonitor
    {
        Task<IEnumerable<IServer>> Servers(ServerFilter filter, CancellationToken token = default);
        Task<IEnumerable<IOrchestratedJob>> OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token = default);
    }
    
    public interface IOrchestratorMonitor : IOrchestratedJobMonitor
    {
        Task<Page<IServer>> Servers(ServerFilter filter, CancellationToken token = default);
        Task<Page<IOrchestratedJob>> OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token = default);
       
    }

    public interface IOrchestratedJobMonitor
    {
        Task<IOrchestratedJob> OrchestratedJob(Guid id, CancellationToken token = default);
    }
}