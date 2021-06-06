using AutoMapper;
using ServerManager.v1.Models.Paging;
using QueryModels = OrchestratR.ServerManager.Domain.Queries.QueryModels;
using Domain = OrchestratR.ServerManager.Domain.Models;

namespace ServerManager.v1.Models.Mapping
{
    internal class DomainToApiProfile : Profile
    {
        public DomainToApiProfile()
        {
            CreateMap(typeof(OrchestratR.Core.Paging.Page<>), typeof(Page<>));
            
            CreateMap<QueryModels.IServer, Server>();
            CreateMap<QueryModels.IOrchestratedJob, ShortJob>();

            CreateMap<QueryModels.IOrchestratedJob, Job>()
                .ForMember(dest => dest.Server, opt => opt.Condition(source => source.Status == Domain.JobLifecycleStatus.Processing));
            CreateMap<QueryModels.IServerReference, ServerReference>();
        }
    }
}