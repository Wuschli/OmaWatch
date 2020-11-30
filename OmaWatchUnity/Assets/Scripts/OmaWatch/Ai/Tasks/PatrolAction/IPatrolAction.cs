using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.Ai.Tasks.PatrolAction
{
    public interface IPatrolAction
    {
        Task Execute(AgentBehaviour agent, CancellationToken token);
    }
}