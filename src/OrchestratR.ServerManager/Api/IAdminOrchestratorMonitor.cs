using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Api
{
    /// <summary>
    /// Monitoring, not paginated, so responses can be too heavy.
    /// </summary>
    public interface IAdminOrchestratorMonitor : IOrchestratedJobMonitor
    {
        /// <summary>
        /// Returns all filtered servers.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<IServer>> Servers(ServerFilter filter, CancellationToken token = default);
        
        /// <summary>
        /// Returns all filtered jobs.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<IOrchestratedJob>> OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token = default);
    }
}