using System;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Mapping
{
    internal class ServerMappingModel : Server
    {
        public ServerMappingModel(Guid id, string name, int maxWorkersCount, DateTimeOffset createdAt, DateTimeOffset? modifyAt, bool isDeleted) :
            base(id, name, maxWorkersCount, createdAt, modifyAt, isDeleted)
        {
        }
    }
}