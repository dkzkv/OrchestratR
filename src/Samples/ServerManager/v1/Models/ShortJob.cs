using System;

namespace ServerManager.v1.Models
{
    public class ShortJob
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JobLifecycleStatus Status { get; set; }
    }
}