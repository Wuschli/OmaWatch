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
            var wait = WaitTime;
            while (wait > 0)
            {
                token.ThrowIfCancellationRequested();

                await Task.Yield();
                wait -= Time.deltaTime;
            }
        }
    }
}