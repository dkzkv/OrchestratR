using System;
using OrchestratR.Core.Messages;

namespace OrchestratR.ServerManager.Domain.Common.Messages
{
    internal class StartJobMessage : BaseMessage, IStartJobMessage
    {
        public StartJobMessage(Guid id,string jobName, string argument, DateTimeOffset createdAt)
        {
            Id = id;
            JobName = jobName;
            Argument = argument;
            CreatedAt = createdAt;
        }
        
        public Guid Id { get; }
        public string JobName { get; }
        public string Argument { get; }
        public DateTimeOffset CreatedAt { get; }
    }
}