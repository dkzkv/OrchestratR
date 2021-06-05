using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using OrchestratR.ServerManager.Api;
using ServerManager.v1.Models;
using ServerManager.v1.Models.Paging;

namespace ServerManager.v1.Controllers
{
    [Route("api/v1/admin/monitor")]
    [ApiController]
    public class AdminMonitorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminOrchestratorMonitor _adminOrchestratorMonitor;
        
        public AdminMonitorController([NotNull] IMapper mapper,
            [NotNull] IAdminOrchestratorMonitor adminOrchestratorMonitor)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _adminOrchestratorMonitor = adminOrchestratorMonitor ?? throw new ArgumentNullException(nameof(adminOrchestratorMonitor));
        }

        [HttpGet("servers/")]
        [ProducesResponseType(typeof(Page<Server>), 200)]
        public async Task<IActionResult> Get([FromQuery] ServerFilter filter,CancellationToken token )
        {
            var servers = await _adminOrchestratorMonitor.Servers(_mapper.Map<OrchestratR.ServerManager.Domain.Queries.QueryModels.ServerFilter>(filter),token);
            return Ok(_mapper.Map<Page<Server>>(servers));
        }
        
        [HttpGet("jobs/")]
        [ProducesResponseType(typeof(Page<Job>), 200)]
        public async Task<IActionResult> Get([FromQuery] OrchestratedJobFilter filter,CancellationToken token )
        {
            var domainFilter = _mapper.Map<OrchestratR.ServerManager.Domain.Queries.QueryModels.OrchestratedJobFilter>(filter);
            var jobs = await _adminOrchestratorMonitor.OrchestratedJobs(domainFilter,token);
            return Ok(_mapper.Map<Page<Job>>(jobs));
        }
    }
}