using System;

namespace OrchestratR.Core.Messages
{
    public interface IStopJobMessage : IMessage
    {
        Guid Id { get; }
    }
}