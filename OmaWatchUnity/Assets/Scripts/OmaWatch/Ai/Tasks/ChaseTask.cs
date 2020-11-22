using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Player;
using UnityEditor.UIElements;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class ChaseTask : ITask
    {
        private readonly SuspiciousBehaviour _chaseTarget;
        private readonly CancellationTokenSource _cancellationToken;
        private float _chaseTime;


        public ChaseTask(SuspiciousBehaviour chaseTarget, float chaseTime)
        {
            _chaseTarget = chaseTarget;
            _chaseTime = chaseTime;
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
            _chaseTime -= Time.deltaTime;
            if (_chaseTime <= 0.0f)
            {
                Debug.Log("Chase Timeout!");
                Cancel();
            }

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