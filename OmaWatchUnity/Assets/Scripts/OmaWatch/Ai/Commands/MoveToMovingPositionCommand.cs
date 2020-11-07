using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch.Ai.Commands
{
    public class MoveToMovingPositionCommand : AbstractCommand
    {
        private readonly Transform _target;
        private readonly CancellationToken _token;

        public MoveToMovingPositionCommand(Transform target, CancellationToken token)
        {
            _target = target;
            _token = token;
        }

        public override async Task<CommandResult> Execute(AgentBehaviour agent)
        {
            var nav = agent.GetComponent<NavMeshAgent>();
            if (nav == null)
                return CommandResult.Failed;

            await SetDestination(nav);

            while (nav.remainingDistance > nav.stoppingDistance)
            {
                await Task.Yield();
                _token.ThrowIfCancellationRequested();

                if (Vector3.Distance(nav.destination, _target.position) > 1.0f)
                    await SetDestination(nav);
            }

            return CommandResult.Success;
        }

        private async Task SetDestination(NavMeshAgent nav)
        {
            nav.SetDestination(_target.position);
            nav.enabled = true;
            nav.isStopped = false;

            while (nav.pathPending && !nav.hasPath)
            {
                await Task.Yield();
                _token.ThrowIfCancellationRequested();
            }
        }
    }
}