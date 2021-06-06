using System;
using System.Collections.Generic;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Server which process jobs.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Server id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Server name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Max jobs which server can provide.
        /// </summary>
        public int MaxWorkersCount { get; set; }
        
        /// <summary>
        /// When was server created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        
        /// <summary>
        /// When was server modified.
        /// </summary>
        public DateTimeOffset? ModifyAt { get; set; }
        
        /// <summary>
        /// Jobs which process on this server.
        /// </summary>
        public ICollection<ShortJob> OrchestratedJobs { get; set; }
        
        /// <summary>
        /// If server deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}