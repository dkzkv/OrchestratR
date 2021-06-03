using System;

namespace OrchestratR.ServerManager.Domain.Models
{
    public class OrchestratedJob
    {
        public OrchestratedJob(string name, string argument)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Job name can't be null or empty");
            
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentException("Job argument can't be null or empty");
            
            Id = Guid.NewGuid();
            Name = name;
            Argument = argument;
            CreatedAt = DateTimeOffset.Now;
            Status = JobLifecycleStatus.Created;
        }

        public OrchestratedJob(Guid id, string name, string argument, DateTimeOffset createdAt, DateTimeOffset? modifyAt, JobLifecycleStatus status, Server server)
        {
            Id = id;
            Name = name;
            Argument = argument;
            CreatedAt = createdAt;
            ModifyAt = modifyAt;
            Status = status;
            Server = server;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Argument { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; private set; }
        public JobLifecycleStatus Status { get; private set;}
        public Server Server { get; private set;}
        
        public OrchestratedJob UpdateStatus(JobLifecycleStatus status)
        {
            Status = status;
            ModifyAt = DateTimeOffset.Now;
            return this;
        }
        
        public OrchestratedJob SetServer(Server server)
        {
            Server = server;
            ModifyAt = DateTimeOffset.Now;
            return this;
        }
    }
}