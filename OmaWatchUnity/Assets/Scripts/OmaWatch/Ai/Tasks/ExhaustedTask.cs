using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class ExhaustedTask : ITask
    {
        private readonly float _exhaustedTime;
        private readonly CancellationTokenSource _token = new CancellationTokenSource();
        public AgentBehaviour.AgentState State => AgentBehaviour.AgentState.Exhausted;

        public ExhaustedTask(float exhaustedTime)
        {
            _exhaustedTime = exhaustedTime;
        }

        public async Task<TaskResult> Run(AgentBehaviour agent)
        {
            try
            {
                agent.Nav?.Stop();

                await Task.Delay((int) (_exhaustedTime * 1000));
                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
        }

        public void Update()
        {
        }

        public void Cancel()
        {
            _token.Cancel();
        }

        public void OnCompleted(TaskResult result)
        {
        }
    }
}