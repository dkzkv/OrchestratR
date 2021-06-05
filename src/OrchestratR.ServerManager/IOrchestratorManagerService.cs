using System.Threading;
using System.Threading.Tasks;

namespace OrchestratR.ServerManager
{
    public interface IOrchestratorManagerService
    {
        Task StartAsync(CancellationToken token);
        Task StopAsync(CancellationToken token);
    }
}