using System;
using OrchestratR.Core.Messages;

namespace OrchestratR.Server.Messages
{
    internal class ServerDeletedMessage : BaseMessage, IServerDeletedMessage
    {
        public ServerDeletedMessage(Guid id, DateTimeOffset deletedAt)
        {
            Id = id;
            DeletedAt = deletedAt;
        }

        public Guid Id { get; }
        public DateTimeOffset DeletedAt { get; }
    }
}