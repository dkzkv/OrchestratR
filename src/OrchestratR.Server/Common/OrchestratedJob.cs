using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OrchestratR.Server.Common
{
    public class OrchestratedJob
    {
        private readonly Func<JobArgument, CancellationToken, Func<Task>, IServiceProvider, Task> _jobActionWithServiceProvider;
        private readonly Func<JobArgument, CancellationToken, Func<Task>, Task> _jobAction;

        private readonly bool _withServiceProvider = false;
        
        public OrchestratedJob([NotNull] Func<JobArgument, CancellationToken, Func<Task>,IServiceProvider ,Task> jobAction)
        {
            _jobActionWithServiceProvider = jobAction ?? throw new ArgumentNullException(nameof(jobAction));
            _withServiceProvider = true;
        }
        
        public OrchestratedJob([NotNull] Func<JobArgument, CancellationToken, Func<Task>, Task> jobAction)
        {
            _jobAction = jobAction ?? throw new ArgumentNullException(nameof(jobAction));
        }

        public async Task Execute(JobArgument jobArgument, CancellationToken token, Func<Task> heartBeat, IServiceProvider serviceProvider)
        {
            if (_withServiceProvider)
            {
                await _jobActionWithServiceProvider.Invoke(jobArgument, token, heartBeat, serviceProvider);
            }
            else
            {
                await _jobAction.Invoke(jobArgument, token, heartBeat);
            }
        }
    }
}