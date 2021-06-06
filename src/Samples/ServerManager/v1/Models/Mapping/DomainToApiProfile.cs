using AutoMapper;
using ServerManager.v1.Models.Paging;
using QueryModels = OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace ServerManager.v1.Models.Mapping
{
    internal class DomainToApiProfile : Profile
    {
        public DomainToApiProfile()
        {
            CreateMap(typeof(OrchestratR.Core.Paging.Page<>), typeof(Page<>));
            
            CreateMap<QueryModels.IServer, Server>();
            CreateMap<QueryModels.IOrchestratedJob, ShortJob>();

            CreateMap<QueryModels.IOrchestratedJob, Job>();
            CreateMap<QueryModels.IServerReference, ServerReference>();
            //.IncludeAllDerived()
        }
    }
}