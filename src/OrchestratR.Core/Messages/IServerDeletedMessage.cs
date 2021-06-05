using System;

namespace OrchestratR.Core.Messages
{
    public interface IServerDeletedMessage : IMessage
    {
        Guid Id { get; }
        DateTimeOffset DeletedAt { get; }
    }
}