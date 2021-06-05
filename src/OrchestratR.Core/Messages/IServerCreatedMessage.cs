using System;

namespace OrchestratR.Core.Messages
{
    public interface IServerCreatedMessage : IMessage
    {
        Guid Id { get; }
        string Name { get; }
        int MaxWorkersCount { get; }
        DateTimeOffset CreatedAt { get; }
    }
}