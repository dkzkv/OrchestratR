using System.Threading;
using System.Threading.Tasks;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Api
{
    /// <summary>
    /// Monitoring, paginated.
    /// </summary>
    public interface IOrchestratorMonitor : IOrchestratedJobMonitor
    {
        /// <summary>
        /// Returns page of filtered servers.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Page<IServer>> Servers(ServerFilter filter, CancellationToken token = default);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Page<IOrchestratedJob>> OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token = default);
       
    }
}