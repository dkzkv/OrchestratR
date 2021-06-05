using System;
using System.Collections.Generic;

namespace ServerManager.v1.Models
{
    public class Server
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public ICollection<ShortJob> OrchestratedJobs { get; set; }
        public bool IsDeleted { get; set; }
    }
}