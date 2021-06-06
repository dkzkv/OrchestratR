using System;
using JetBrains.Annotations;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Persistence.Mapping.AuxiliaryModels
{
    [UsedImplicitly]
    internal class ServerReferenceAuxiliaryModel : IServerReference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}