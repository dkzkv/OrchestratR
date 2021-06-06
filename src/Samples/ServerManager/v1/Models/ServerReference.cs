using System;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Server reference, same object as server.
    /// </summary>
    public class ServerReference
    {
        /// <summary>
        /// Server id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Server name
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
    }
}