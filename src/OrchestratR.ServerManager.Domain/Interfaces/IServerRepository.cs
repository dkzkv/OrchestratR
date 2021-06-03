using System;
using System.Threading;
using System.Threading.Tasks;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Interfaces
{
    public interface IServerRepository
    {
        Task<Server> GetAsync(Guid id, CancellationToken token = default);
        Task CreateAsync(Server server, CancellationToken token = default);
        Task UpdateAsync(Server server, CancellationToken token = default);
    }
}