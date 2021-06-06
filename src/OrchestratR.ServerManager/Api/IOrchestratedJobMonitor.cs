using System;
using System.Threading;
using System.Threading.Tasks;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Api
{
    /// <summary>
    /// Concrete job monitoring.
    /// </summary>
    public interface IOrchestratedJobMonitor
    {
        /// <summary>
        /// Get concrete job information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IOrchestratedJob> OrchestratedJob(Guid id, CancellationToken token = default);
    }
}