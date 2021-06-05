using System.Collections.Generic;
using MediatR;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Domain.Queries
{
    public class AdminOrchestratedJobQuery  : IRequest<IEnumerable<IOrchestratedJob>>
    {
        public OrchestratedJobFilter Filter { get; }
        
        public AdminOrchestratedJobQuery(OrchestratedJobFilter filter)
        {
            Filter = filter;
        }
    }
}