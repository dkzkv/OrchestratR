using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OrchestratR.Server.Common
{
    public class OrchestratedJob
    {
        private readonly Func<string, string, CancellationToken, Task> _jobAction;

        public OrchestratedJob([NotNull] Func<string, string, CancellationToken, Task> jobAction)
        {
            _jobAction = jobAction ?? throw new ArgumentNullException(nameof(jobAction));
        }

        public async Task Execute(string name, string argument, CancellationToken token)
        {
            await _jobAction.Invoke(name, argument, token);
        }
    }
}