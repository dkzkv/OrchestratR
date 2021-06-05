using AutoMapper;

namespace OrchestratR.ServerManager.Persistence.Handlers
{
    public abstract class BaseQueryHandler
    {
        protected BaseQueryHandler(OrchestratorDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        protected  OrchestratorDbContext Context { get; }
        protected  IMapper Mapper { get; }
    }
}