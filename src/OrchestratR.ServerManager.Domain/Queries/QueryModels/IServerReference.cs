using System;

namespace OrchestratR.ServerManager.Domain.Queries.QueryModels
{
    public interface IServerReference
    {
        public Guid Id { get; }
        public string Name { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public bool IsDeleted { get; }
    }
}