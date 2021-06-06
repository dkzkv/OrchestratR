using System;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Orchestrated job.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Job id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Unique job name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Argument which was passed to job.
        /// </summary>
        public string Argument { get; set; }
        
        /// <summary>
        /// Creation time
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        
        /// <summary>
        /// Last job modification time.
        /// </summary>
        public DateTimeOffset? ModifyAt { get; set; }
        
        /// <summary>
        /// Heart beat time. 
        /// </summary>
        public DateTimeOffset? HeartBeatTime { get; }
        
        /// <summary>
        /// Job lifecycle status.
        /// </summary>
        public JobLifecycleStatus Status { get; set; }
        
        /// <summary>
        /// Server which process current job.
        /// </summary>
        public ServerReference Server { get; set; }
    }
}