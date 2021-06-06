using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrchestratR.ServerManager.Api
{
    /// <summary>
    /// Client to maintain jobs.
    /// </summary>
    public interface IOrchestratorClient
    {
        /// <summary>
        /// Start new job on server. 
        /// </summary>
        /// <param name="name">unique name job</param>
        /// <param name="argument">any argument for your job logic</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Guid> CreateJob(string name, string argument, CancellationToken token = default);

        /// <summary>
        /// Cancel job, will shift job in OnDeleting status, and invoke cancellation token.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task MarkOnDeleting(Guid id, CancellationToken token = default);
    }
}