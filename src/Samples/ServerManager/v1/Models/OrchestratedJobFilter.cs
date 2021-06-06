using ServerManager.v1.Models.Paging;

namespace ServerManager.v1.Models
{
    
    /// <summary>
    /// Job filter
    /// </summary>
    public class OrchestratedJobFilter : PageFilter
    {
        /// <summary>
        /// By job name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// By current job status.
        /// </summary>
        public JobLifecycleStatus? Status { get; set; }
        
        /// <summary>
        /// Jobs except status.
        /// </summary>
        public JobLifecycleStatus? ExceptStatus { get; set; }
    }
}