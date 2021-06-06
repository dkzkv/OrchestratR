using System;
using JetBrains.Annotations;
using OrchestratR.ServerManager.Domain.Models;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Persistence.Mapping.AuxiliaryModels
{
    [UsedImplicitly]
    internal class OrchestratedJobAuxiliaryModel : IOrchestratedJob
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Argument { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public DateTimeOffset? HeartBeatTime { get; }
        public JobLifecycleStatus Status { get; set; }
        public IServerReference Server { get; set; }
    }
}