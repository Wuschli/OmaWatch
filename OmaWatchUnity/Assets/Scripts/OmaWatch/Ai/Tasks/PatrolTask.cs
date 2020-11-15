using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class PatrolTask : AbstractWorldTask
    {
        private CancellationTokenSource _cancellationTokenSource;
        public Transform[] PatrolPoints; 

        public override async Task<TaskResult> Run(AgentBehaviour agent)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                foreach (var patrolPoint in PatrolPoints)
                {
                    await agent.ExecuteCommand(new MoveToPositionCommand(patrolPoint, _cancellationTokenSource.Token));

                    //TODO: patrol actions
                }

                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
        }

        public override void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}