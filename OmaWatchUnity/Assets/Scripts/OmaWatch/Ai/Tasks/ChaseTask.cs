using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using UnityEditor.UIElements;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class ChaseTask : ITask
    {
        private readonly Transform _chaseTarget;
        private readonly float _chaseTime;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly Random _random = new Random();

        public ChaseTask(Transform chaseTarget, float chaseTime)
        {
            _chaseTarget = chaseTarget;
            _chaseTime = chaseTime;
            _cancellationToken = new CancellationTokenSource();
        }

        public async Task<TaskResult> Run(AgentBehaviour agent)
        {
            try
            {
                await agent.ExecuteCommand(new MoveToMovingPositionCommand(_chaseTarget, _chaseTime, _cancellationToken.Token));
                Debug.Log("The chase has completed");
                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
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