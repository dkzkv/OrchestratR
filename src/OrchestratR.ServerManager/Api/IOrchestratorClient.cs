using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrchestratR.ServerManager.Api
{
    public interface IOrchestratorClient
    {
        Task<Guid> CreateJob(string name, string argument, CancellationToken token = default);

        Task MarkOnDeleting(Guid id, CancellationToken token = default);
    }
}