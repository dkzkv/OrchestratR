using AutoMapper;
using OrchestratR.ServerManager.Domain.Models;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;
using OrchestratR.ServerManager.Persistence.Mapping.AuxiliaryModels;

namespace OrchestratR.ServerManager.Persistence.Mapping
{
    public class PersistenceToDomainProfile : Profile
    {
        public PersistenceToDomainProfile()
        {
            CreateMap<Entities.Server, Server>()
                .ConstructUsing((o, _) => new Server(o.Id,
                    o.Name,
                    o.MaxWorkersCount,
                    o.CreatedAt,
                    o.ModifyAt,
                    o.IsDeleted));

            CreateMap<Entities.OrchestratedJob, OrchestratedJob>()
                .ConstructUsing((o, m) => new OrchestratedJob(o.Id,
                    o.Name,
                    o.Argument,
                    o.CreatedAt,
                    o.ModifyAt,
                    o.Status,
                    m.Mapper.Map<Server>(o.Server)));

            CreateMap<Entities.Server, IServer>().As<ServerAuxiliaryModel>();
            CreateMap<Entities.Server, ServerAuxiliaryModel>();

            CreateMap<Entities.Server, IServerReference>().As<ServerReferenceAuxiliaryModel>();
            CreateMap<Entities.Server, ServerReferenceAuxiliaryModel>();

            CreateMap<Entities.OrchestratedJob, IOrchestratedJob>().As<OrchestratedJobAuxiliaryModel>();
            CreateMap<Entities.OrchestratedJob, OrchestratedJobAuxiliaryModel>();
        }
    }
}