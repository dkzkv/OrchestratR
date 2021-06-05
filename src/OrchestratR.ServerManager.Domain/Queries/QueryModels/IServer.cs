using System;
using System.Collections.Generic;

namespace OrchestratR.ServerManager.Domain.Queries.QueryModels
{
    public interface IServer
    {
        public Guid Id { get; }
        public string Name { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public ICollection<IOrchestratedJob> OrchestratedJobs { get; }
        public bool IsDeleted { get; }
    }
}