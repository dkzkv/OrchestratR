using AutoMapper;

namespace OrchestratR.ServerManager.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected BaseRepository(OrchestratorDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected  OrchestratorDbContext Context { get; }
        protected  IMapper Mapper { get; }
    }
}