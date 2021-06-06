using System;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Mapping
{
    internal class OrchestratedJobMappingModel : OrchestratedJob
    {
        public OrchestratedJobMappingModel(Guid id, string name, string argument, DateTimeOffset createdAt, DateTimeOffset? modifyAt,
            DateTimeOffset? heartBeatTime, JobLifecycleStatus status, Server server) : base(id, name, argument, createdAt, modifyAt, heartBeatTime,
            status, server)
        {
        }
    }
}