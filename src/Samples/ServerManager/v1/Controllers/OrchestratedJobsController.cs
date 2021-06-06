using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using OrchestratR.ServerManager.Api;
using ServerManager.v1.Models;

namespace ServerManager.v1.Controllers
{
    [Route("api/v1/jobs")]
    [ApiController]
    public class OrchestratedJobsController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrchestratorClient _orchestratorClient;
        private readonly IOrchestratorMonitor _orchestratorMonitor;

        public OrchestratedJobsController([NotNull] IMapper mapper,
            [NotNull] IOrchestratorClient orchestratorClient,
            [NotNull] IOrchestratorMonitor orchestratorMonitor)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orchestratorClient = orchestratorClient ?? throw new ArgumentNullException(nameof(orchestratorClient));
            _orchestratorMonitor = orchestratorMonitor ?? throw new ArgumentNullException(nameof(orchestratorMonitor));
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(Job), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
           var job = await _orchestratorMonitor.OrchestratedJob(id,token);
            return Ok(_mapper.Map<Job>(job));
        }
        
        [HttpPost("")]
        [ProducesResponseType(typeof(Job), 200)]
        public async Task<IActionResult> Post([FromBody] CreateJobArgument job,CancellationToken token )
        {
            var jobId = await _orchestratorClient.CreateJob(job.Name, job.Argument,token);
            var createdJob = await _orchestratorMonitor.OrchestratedJob(jobId,token);
            return Ok(_mapper.Map<Job>(createdJob));
        }
        
        [HttpDelete("id")]
        [ProducesResponseType(typeof(Job), 200)]
        public async Task<IActionResult> Post([FromRoute] Guid id,CancellationToken token )
        {
            await _orchestratorClient.MarkOnDeleting(id,token);
            return Ok();
        }
    }
}