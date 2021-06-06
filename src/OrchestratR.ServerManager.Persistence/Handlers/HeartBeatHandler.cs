using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchestratR.ServerManager.Domain.Commands;

namespace OrchestratR.ServerManager.Persistence.Handlers
{
    public class HeartBeatHandler : BaseHandler, IRequestHandler<UpdateHeartBeatCommand>
    {
        private readonly ILogger<HeartBeatHandler> _logger;

        public HeartBeatHandler(OrchestratorDbContext context, IMapper mapper, ILogger<HeartBeatHandler> logger) : base(context, mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateHeartBeatCommand request, CancellationToken cancellationToken)
        {
            var job = await Context.OrchestratedJobs
                .FirstOrDefaultAsync(o => o.Id == request.JobId, cancellationToken);

            if (job is null)
            {
                _logger.LogWarning($"Can't set heat beat time to job {request.JobId}, job not existed.");
                return Unit.Value;
            }

            job.HeartBeatTime = request.HeartBeatTime;
            
            await Context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}