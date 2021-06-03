using AutoMapper;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Mapping
{
    public class PersistenceToDomainProfile : Profile
    {
        public PersistenceToDomainProfile()
        {
            CreateMap<Entities.Server, Server>()
                .ConstructUsing((o,m) => new Server(o.Id,
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
        }
    }
}