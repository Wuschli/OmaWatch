using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class GrabTask : ITask
    {
        private readonly GameObject _subject;
        private readonly Transform _releaseTarget;
        private readonly CancellationTokenSource _token;

        public GrabTask(GameObject subject, Transform releaseReleaseTarget)
        {
            _subject = subject;
            _releaseTarget = releaseReleaseTarget;
            _token = new CancellationTokenSource();
        }

        public async Task<TaskResult> Run(AgentBehaviour agent)
        {
            try
            {
                SetPlayerEnabled(false);
                _subject.transform.parent = agent.transform;    //TODO: specify grab transform

                await agent.ExecuteCommand(new MoveToPositionCommand(_releaseTarget.position, _token.Token));
                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
        }

        public void Cancel()
        {
            _token.Cancel();
        }

        public void OnCompleted(TaskResult result)
        {
            SetPlayerEnabled(true);
            _subject.transform.parent = null;
        }

        private void SetPlayerEnabled(bool enabled)
        {
            var player = _subject.GetComponent<PlayerController>();
            if (player != null)
                player.enabled = enabled;
        }
    }
}