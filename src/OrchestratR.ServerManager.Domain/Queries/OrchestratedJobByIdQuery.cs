using System;
using MediatR;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Domain.Queries
{
    public class OrchestratedJobByIdQuery :  IRequest<IOrchestratedJob>
    {
        public Guid Id { get; }

        public OrchestratedJobByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}