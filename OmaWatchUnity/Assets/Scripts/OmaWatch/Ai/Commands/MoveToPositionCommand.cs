using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Commands
{
    public class MoveToPositionCommand : AbstractCommand
    {
        private readonly Transform _target;
        private readonly CancellationToken _token;

        public MoveToPositionCommand(Transform target, CancellationToken token)
        {
            _target = target;
            _token = token;
        }

        public override async Task<CommandResult> Execute(AgentBehaviour agent)
        {
            var nav = agent.Nav;
            if (nav == null)
                return CommandResult.Failed;

            var result = await nav.MoveToTarget(_target, _token);
            switch (result)
            {
                case TileNavMovementController.MoveResult.Success:
                    return CommandResult.Success;
                case TileNavMovementController.MoveResult.Canceled:
                    return CommandResult.Cancelled;
                default:
                    return CommandResult.Failed;
            }
        }
    }
}