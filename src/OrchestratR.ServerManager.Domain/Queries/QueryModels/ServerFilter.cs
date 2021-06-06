using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Queries.QueryModels
{
    public class ServerFilter : PageFilter
    {
        public bool? IsDeleted { get; set; }
    }
}