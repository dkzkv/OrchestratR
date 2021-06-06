using AutoMapper;

namespace OrchestratR.ServerManager.Persistence.Handlers
{
    public abstract class BaseHandler
    {
        protected BaseHandler(OrchestratorDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        protected  OrchestratorDbContext Context { get; }
        protected  IMapper Mapper { get; }
    }
}