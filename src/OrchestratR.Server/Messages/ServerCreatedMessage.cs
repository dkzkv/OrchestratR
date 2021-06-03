using System;
using OrchestratR.Core.Messages;

namespace OrchestratR.Server.Messages
{
    internal class ServerCreatedMessage : BaseMessage, IServerCreatedMessage
    {
        public ServerCreatedMessage(Guid id, string name, int maxWorkersCount, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            MaxWorkersCount = maxWorkersCount;
            CreatedAt = createdAt;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
    }
}