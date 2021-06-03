using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrchestratR.ServerManager.Domain
{
    public interface IJobOrchestrationClient
    {
        Task<Guid> Create(string name, string argument, CancellationToken token = default);
        Task MarkOnDeleting(Guid id, CancellationToken token = default);
    }
}