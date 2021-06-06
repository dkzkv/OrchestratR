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
        Created,
        
        /// <summary>
        /// Processing.
        /// </summary>
        Processing,
        
        /// <summary>
        /// OnDeleting.
        /// </summary>
        OnDeleting,
        
        /// <summary>
        /// Deleted.
        /// </summary>
        Deleted
    }
}