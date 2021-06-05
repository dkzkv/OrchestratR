using System;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Domain.Queries.QueryModels
{
    public interface IOrchestratedJob
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Argument { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public JobLifecycleStatus Status { get; }
        public IServerReference Server { get; }
    }
}