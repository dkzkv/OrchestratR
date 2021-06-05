using AutoMapper;
using QueryModels = OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace ServerManager.v1.Models.Mapping
{
    public class ApiToDomainProfile : Profile
    {
        public ApiToDomainProfile()
        {
            CreateMap<ServerFilter, QueryModels.ServerFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null))
                .IncludeAllDerived();

            CreateMap<OrchestratedJobFilter, QueryModels.OrchestratedJobFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null))
                .IncludeAllDerived();
        }
    }
}