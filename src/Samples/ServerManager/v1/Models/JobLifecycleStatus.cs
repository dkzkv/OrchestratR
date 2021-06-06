using System.ComponentModel;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Job lifecycle statuses.
    /// </summary>
    public enum JobLifecycleStatus
    {
        /// <summary>
        /// Created.
        /// </summary>
        [Description("Created")]
        Created,
        
        /// <summary>
        /// Processing.
        /// </summary>
        [Description("Processing")]
        Processing,
        
        /// <summary>
        /// OnDeleting.
        /// </summary>
        [Description("OnDeleting")]
        OnDeleting,
        
        /// <summary>
        /// Deleted.
        /// </summary>
        [Description("Deleted")]
        Deleted
    }
}