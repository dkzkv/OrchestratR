using System;

namespace OrchestratR.ServerManager.Domain.Models
{
    public class Server
    {
        public Server(Guid id,string name, int maxWorkersCount, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            MaxWorkersCount = maxWorkersCount;
            CreatedAt = createdAt;
            ModifyAt = DateTimeOffset.Now;
        }
        
        protected Server(Guid id, string name, int maxWorkersCount, DateTimeOffset createdAt, DateTimeOffset? modifyAt, bool isDeleted)
        {
            Id = id;
            Name = name;
            MaxWorkersCount = maxWorkersCount;
            CreatedAt = createdAt;
            ModifyAt = modifyAt;
            IsDeleted = isDeleted;
        }
        
        public Guid Id { get; }
        public string Name { get; }
        public int MaxWorkersCount { get;  }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; private set; }
        public bool IsDeleted { get; private set;}
        
        public Server SetAsDeleted()
        {
            IsDeleted = true;
            ModifyAt = DateTimeOffset.Now;

            return this;
        }
    }
}