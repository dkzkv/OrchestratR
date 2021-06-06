using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Models;
using OrchestratR.ServerManager.Domain.Queries;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;
using OrchestratR.ServerManager.Persistence.Extensions;
using OrchestratR.ServerManager.Persistence.Handlers.Specs;

namespace OrchestratR.ServerManager.Persistence.Handlers
{
    public class ServerHandler : BaseHandler, IRequestHandler<ServerQuery,Page<IServer>>,
        IRequestHandler<AdminServerQuery,IEnumerable<IServer>>
    {
        public ServerHandler(OrchestratorDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        
        public async Task<Page<IServer>> Handle(ServerQuery request, CancellationToken cancellationToken)
        {
            return await BaseServerQuery(request.Filter)
                .OrderBy(o => o.Name)
                .Page(request.Filter, Mapper.Map<IServer>, cancellationToken);
        }

        public async Task<IEnumerable<IServer>> Handle(AdminServerQuery request, CancellationToken cancellationToken)
        {
            var result = await BaseServerQuery(request.Filter)
                .ToArrayAsync(cancellationToken);
            
            return Mapper.Map<IEnumerable<IServer>>(result);
        }

        private IQueryable<Entities.Server> BaseServerQuery(ServerFilter filter)
        {
            return Context.Servers
                .AsNoTracking()
                .Include(o => o.OrchestratedJobs)
                .Where(ServerSpecs.ByIsDeleted(filter.IsDeleted))
                .Select(o =>
                    new Entities.Server()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        MaxWorkersCount = o.MaxWorkersCount,
                        CreatedAt = o.CreatedAt,
                        ModifyAt = o.ModifyAt,
                        IsDeleted = o.IsDeleted,
                        OrchestratedJobs = o.OrchestratedJobs
                            .Where(j=>j.Status != JobLifecycleStatus.Deleted)
                            .ToArray()
                    }
                );
        }
    }
}