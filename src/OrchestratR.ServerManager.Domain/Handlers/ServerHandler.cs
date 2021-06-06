using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using OrchestratR.ServerManager.Domain.Commands;
using OrchestratR.ServerManager.Domain.Interfaces;

namespace OrchestratR.ServerManager.Domain.Handlers
{
    public class ServerHandler : IRequestHandler<RecordServerCommand>,
        IRequestHandler<MarkAsDeletedServerCommand>
    {
        private readonly IServerRepository _serverRepository;

        public ServerHandler([NotNull] IServerRepository serverRepository)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        }

        public async Task<Unit> Handle(RecordServerCommand request, CancellationToken token)
        {
            var existedServer = await _serverRepository.GetAsync(request.Server.Id, token);
            if (existedServer is null)
            {
                await _serverRepository.CreateAsync(request.Server, token);
            }
            
            await _serverRepository.UpdateAsync(request.Server, token);
            return Unit.Value;
        }
        
        public async Task<Unit> Handle(MarkAsDeletedServerCommand request, CancellationToken token)
        {
            var existedServer = await _serverRepository.GetAsync(request.Id, token);
            if(existedServer is null)
                throw new InvalidOperationException("Can't set server as deleted, not exist.");
            
            await _serverRepository.UpdateAsync(existedServer.SetAsDeleted(), token);
            return Unit.Value;
        }
    }
}