using System;

namespace ServerManager.v1.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Argument { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public JobLifecycleStatus Status { get; set; }
        public ServerReference Server { get; set; }
    }
}