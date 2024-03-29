using MediatR;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Domain.Queries
{
    public class ServerQuery : IRequest<Page<IServer>>
    {
        public ServerFilter Filter { get; }

        public ServerQuery(ServerFilter filter)
        {
            Filter = filter;
        }
    }
}