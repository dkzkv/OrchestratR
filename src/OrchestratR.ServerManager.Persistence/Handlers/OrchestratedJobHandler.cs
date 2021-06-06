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

namespace OrchestratR.ServerManager.Persistence.Handlers
{
    public class OrchestratedJobHandler : BaseHandler, IRequestHandler<OrchestratedJobByIdQuery,IOrchestratedJob>,
        IRequestHandler<OrchestratedJobQuery,Page<IOrchestratedJob>>,
        IRequestHandler<AdminOrchestratedJobQuery,IEnumerable<IOrchestratedJob>>
    {
        public OrchestratedJobHandler(OrchestratorDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IOrchestratedJob> Handle(OrchestratedJobByIdQuery request, CancellationToken cancellationToken)
        {
            var dist = await Context.OrchestratedJobs
                .AsNoTracking()
                .Include(o=>o.Server)
                .FirstOrDefaultAsync(o => o.Id.Equals(request.Id),cancellationToken);

            return Mapper.Map<IOrchestratedJob>(dist);
        }

        public async Task<Page<IOrchestratedJob>> Handle(OrchestratedJobQuery request, CancellationToken cancellationToken)
        {
            return await Context.OrchestratedJobs
                .AsNoTracking()
                .Include(o=>o.Server)
                .Where(JobSpecs.ByName(request.Filter.Name))
                .Where(JobSpecs.ByStatus(request.Filter.Status))
                .Where(JobSpecs.ByExceptStatus(request.Filter.ExceptStatus))
                .OrderBy(o=>o.Name)
                .Page(request.Filter,Mapper.Map<IOrchestratedJob>, cancellationToken);
        }

        public async Task<IEnumerable<IOrchestratedJob>> Handle(AdminOrchestratedJobQuery request, CancellationToken cancellationToken)
        {
            var result = await Context.OrchestratedJobs
                .AsNoTracking()
                .Include(o => o.Server)
                .Where(JobSpecs.ByName(request.Filter.Name))
                .Where(JobSpecs.ByStatus(request.Filter.Status))
                .Where(JobSpecs.ByExceptStatus(request.Filter.ExceptStatus))
                .ToArrayAsync(cancellationToken);
            return Mapper.Map<IEnumerable<IOrchestratedJob>>(result);
        }
    }
}