using System.Threading;
using System.Threading.Tasks;
using OrchestratR.Core.Messages;

namespace OrchestratR.Core.Publishers
{
    public interface IServerPublisher
    {
        Task Send(IServerCreatedMessage message, CancellationToken token = default);
        Task Send(IServerDeletedMessage message, CancellationToken token = default);
        Task Send(IJobStatusMessage message, CancellationToken token = default);
        Task Publish(IJobHearBeatMessage message, CancellationToken token = default);
    }
}