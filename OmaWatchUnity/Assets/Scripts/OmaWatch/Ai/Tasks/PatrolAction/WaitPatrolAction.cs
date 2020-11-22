using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks.PatrolAction
{
    public class WaitPatrolAction : MonoBehaviour, IPatrolAction
    {
        public float WaitTime;

        public async Task Execute(AgentBehaviour agent, CancellationToken token)
        {
            await Task.Delay((int) (1000 * WaitTime), token);
        }
    }
}