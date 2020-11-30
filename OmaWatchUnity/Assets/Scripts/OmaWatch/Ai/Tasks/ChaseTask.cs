using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Player;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class ChaseTask : ITask
    {
        private readonly SuspiciousBehaviour _chaseTarget;
        private readonly CancellationTokenSource _cancellationToken;


        public ChaseTask(SuspiciousBehaviour chaseTarget)
        {
            _chaseTarget = chaseTarget;
            _cancellationToken = new CancellationTokenSource();
        }

        public AgentBehaviour.AgentState State => AgentBehaviour.AgentState.Chase;

        public async Task<TaskResult> Run(AgentBehaviour agent)
        {
            try
            {
                var result = await agent.ExecuteCommand(new MoveToPositionCommand(_chaseTarget.transform, _cancellationToken.Token));
                Debug.Log($"Chase MoveTo: {result}");
                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
        }

        public void Update()
        {
            if (!_chaseTarget.IsSuspicious)
                Cancel();
        }

        public void Cancel()
        {
            _cancellationToken.Cancel();
        }

        public void OnCompleted(TaskResult result)
        {
        }
    }
}