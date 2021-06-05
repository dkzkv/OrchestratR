using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using OrchestratR.ServerManager.Api;
using ServerManager.v1.Models;
using ServerManager.v1.Models.Paging;
using QueryModels = OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace ServerManager.v1.Controllers
{
    [Route("api/v1/monitor")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrchestratorMonitor _orchestratorMonitor;
        
        public MonitorController([NotNull] IMapper mapper,
            [NotNull] IOrchestratorMonitor orchestratorMonitor)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orchestratorMonitor = orchestratorMonitor ?? throw new ArgumentNullException(nameof(orchestratorMonitor));
        }

        [HttpGet("servers/")]
        [ProducesResponseType(typeof(Page<Server>), 200)]
        public async Task<IActionResult> Get([FromQuery] ServerFilter filter,CancellationToken token )
        {
            var servers = await _orchestratorMonitor.Servers(_mapper.Map<QueryModels.ServerFilter>(filter),token);
            return Ok(_mapper.Map<Page<Server>>(servers));
        }
        
        [HttpGet("jobs/")]
        [ProducesResponseType(typeof(Page<Job>), 200)]
        public async Task<IActionResult> Get([FromQuery] OrchestratedJobFilter filter,CancellationToken token )
        {
            var domainFilter = _mapper.Map<QueryModels.OrchestratedJobFilter>(filter);
            var jobs = await _orchestratorMonitor.OrchestratedJobs(domainFilter,token);
            return Ok(_mapper.Map<Page<Job>>(jobs));
        }
    }
}