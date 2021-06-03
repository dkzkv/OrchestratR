using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain
{
    public class ServerService
    {
        public ServerService([NotNull] IServerRepository serverRepository)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        }

        private readonly IServerRepository _serverRepository;

        public async Task Record(Server server, CancellationToken token = default)
        {
            var existedServer = await _serverRepository.GetAsync(server.Id, token);
            if (existedServer is null)
            {
                await _serverRepository.CreateAsync(server, token);
            }
            
            await _serverRepository.UpdateAsync(server, token);
        }

        public async Task MarkAsDeleted(Guid id, CancellationToken token = default)
        {
            var existedServer = await _serverRepository.GetAsync(id, token);
            if(existedServer is null)
                throw new InvalidOperationException("Can't set server as deleted, not exist.");
            
            await _serverRepository.UpdateAsync(existedServer.SetAsDeleted(), token);
        }
    }
}