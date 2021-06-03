using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Repositories
{
    public class ServerRepository : BaseRepository, IServerRepository
    {
        public ServerRepository(OrchestratorDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Server> GetAsync(Guid id, CancellationToken token = default)
        {
            var result = await Context.Servers.FirstOrDefaultAsync(o => o.Id.Equals(id), token);
            return Mapper.Map<Server>(result);
        }

        public async Task CreateAsync(Server server, CancellationToken token = default)
        {
            var existedServer = await GetAsync(server.Id, token);
            if(existedServer != null)
                throw new InvalidOperationException("Server with same id already exist!");

            await Context.Servers.AddAsync(Mapper.Map<Entities.Server>(server), token);
            await Context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Server server, CancellationToken token = default)
        {
            var existedServer = await Context.Servers.FirstOrDefaultAsync(o => o.Id.Equals(server.Id), token);
            if(existedServer == null)
                throw new InvalidOperationException("Server with same id is not exist!");

            existedServer.Name = server.Name;
            existedServer.MaxWorkersCount = server.MaxWorkersCount;
            existedServer.CreatedAt = server.CreatedAt;
            existedServer.ModifyAt = server.ModifyAt;
            existedServer.IsDeleted = server.IsDeleted;
           
            await Context.SaveChangesAsync(token);
        }
    }
}