using MediatR;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Domain.Queries
{
    public class OrchestratedJobQuery  : IRequest<Page<IOrchestratedJob>>
    {
        public OrchestratedJobFilter Filter { get; }
        
        public OrchestratedJobQuery(OrchestratedJobFilter filter)
        {
            Filter = filter;
        }
    }
}