using System;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Short job meta information
    /// </summary>
    public class ShortJob
    {
        /// <summary>
        /// Job id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Job name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Job lifecycle status.
        /// </summary>
        public JobLifecycleStatus Status { get; set; }
    }
}