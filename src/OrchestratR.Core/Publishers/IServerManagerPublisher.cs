using System.Threading;
using System.Threading.Tasks;
using OrchestratR.Core.Messages;

namespace OrchestratR.Core.Publishers
{
    public interface IServerManagerPublisher
    {
        Task Send(IStartJobMessage message, CancellationToken token = default);
        Task Publish(IStopJobMessage message, CancellationToken token = default);
    }
}