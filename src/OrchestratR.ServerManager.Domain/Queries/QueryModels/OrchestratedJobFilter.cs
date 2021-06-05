using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Queries.QueryModels
{
    public class OrchestratedJobFilter : PageFilter
    {
        public string Name { get; set; }
        public JobLifecycleStatus? Status { get; set; }
        public JobLifecycleStatus? ExceptStatus { get; set; }
    }
}