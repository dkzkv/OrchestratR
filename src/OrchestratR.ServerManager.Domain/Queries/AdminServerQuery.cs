using System.Collections.Generic;
using MediatR;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Domain.Queries
{
    public class AdminServerQuery : IRequest<IEnumerable<IServer>>
    {
        public ServerFilter Filter { get; }

        public AdminServerQuery(ServerFilter filter)
        {
            Filter = filter;
        }
    }
}