using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch.Ai.Commands
{
    public class MoveToPositionCommand : AbstractCommand
    {
        private readonly Vector3 _position;
        private readonly CancellationToken _token;

        public MoveToPositionCommand(Vector3 position, CancellationToken token)
        {
            _position = position;
            _token = token;
        }

        public override async Task<CommandResult> Execute(AgentBehaviour agent)
        {
            var nav = agent.GetComponent<NavMeshAgent>();
            if (nav == null)
                return CommandResult.Failed;

            nav.SetDestination(_position);
            nav.enabled = true;
            nav.isStopped = false;

            while (nav.pathPending && !nav.hasPath)
            {
                await Task.Yield();
                _token.ThrowIfCancellationRequested();
            }

            while (nav.remainingDistance > nav.stoppingDistance)
            {
                await Task.Yield();
                _token.ThrowIfCancellationRequested();
            }

            return CommandResult.Success;
        }
    }
}