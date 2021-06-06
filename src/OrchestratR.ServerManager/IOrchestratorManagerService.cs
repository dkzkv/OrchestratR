using System.Threading;
using System.Threading.Tasks;

namespace OrchestratR.ServerManager
{
    /// <summary>
    /// Use this if your service not HostBuilder.
    /// </summary>
    public interface IOrchestratorManagerService
    {
        /// <summary>
        /// Start orchestrator manager. Run message provider. Prepare infrastructure for manager such as migrations etc...
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task StartAsync(CancellationToken token);
        
        /// <summary>
        /// Stop orchestrator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task StopAsync(CancellationToken token);
    }
}