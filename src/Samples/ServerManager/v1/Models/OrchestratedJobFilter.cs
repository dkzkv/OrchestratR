using ServerManager.v1.Models.Paging;

namespace ServerManager.v1.Models
{
    public class OrchestratedJobFilter : PageFilter
    {
        public string Name { get; set; }
        public JobLifecycleStatus? Status { get; set; }
        public JobLifecycleStatus? ExceptStatus { get; set; }
    }
}