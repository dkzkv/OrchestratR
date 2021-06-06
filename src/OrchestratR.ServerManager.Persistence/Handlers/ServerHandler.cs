using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;
using OrchestratR.ServerManager.Persistence.Extensions;
using OrchestratR.ServerManager.Persistence.Handlers.Specs;
using Z.EntityFramework.Plus;

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
            var baseQuery = Context.Servers
                .AsNoTracking();
            if (filter.JobStatus.HasValue)
                baseQuery = baseQuery.IncludeFilter(o => o.OrchestratedJobs.Where(j => j.Status == filter.JobStatus));
            
            if (filter.ExceptJobStatus.HasValue)
                baseQuery = baseQuery.IncludeFilter(o => o.OrchestratedJobs.Where(j => j.Status != filter.ExceptJobStatus));

            return baseQuery.Where(ServerSpecs.ByIsDeleted(filter.IsDeleted));
        }
    }
}