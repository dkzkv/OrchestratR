using System;

namespace ServerManager.v1.Models
{
    public class ServerReference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
    }
}