using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OrchestratR.Server.Common
{
    public class OrchestratedJob
    {
        private readonly Func<JobArgument, CancellationToken, Func<Task> , Task> _jobAction;

        public OrchestratedJob([NotNull] Func<JobArgument, CancellationToken, Func<Task>, Task> jobAction)
        {
            _jobAction = jobAction ?? throw new ArgumentNullException(nameof(jobAction));
        }

        public async Task Execute(JobArgument jobArgument, CancellationToken token, Func<Task> heartBeat)
        {
            await _jobAction.Invoke(jobArgument, token, heartBeat);
        }
    }
}