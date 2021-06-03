using AutoMapper;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Mapping
{
    public class DomainToPersistenceProfile : Profile
    {
        public DomainToPersistenceProfile()
        {
            CreateMap<OrchestratedJob, Entities.OrchestratedJob>();
            CreateMap<Server, Entities.Server>();
        }
    }
}