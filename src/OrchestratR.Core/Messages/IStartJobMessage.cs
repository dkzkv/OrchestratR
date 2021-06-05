using System;

namespace OrchestratR.Core.Messages
{
    public interface IStartJobMessage : IMessage
    {
        Guid Id { get; }
        string JobName { get; }
        string Argument { get; }
        DateTimeOffset CreatedAt { get; }
    }
}