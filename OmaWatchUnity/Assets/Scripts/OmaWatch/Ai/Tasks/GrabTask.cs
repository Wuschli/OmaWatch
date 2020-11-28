using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Player;
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

        public AgentBehaviour.AgentState State => AgentBehaviour.AgentState.Grab;

        public async Task<TaskResult> Run(AgentBehaviour agent)
        {
            try
            {
                SetPlayerGrabbed(true);
                _subject.GetComponentInChildren<ScrapTrail>()?.DropAll();

                _subject.transform.parent = agent.transform;    //TODO: specify grab transform
                _subject.transform.localPosition = Vector3.zero;

                await agent.ExecuteCommand(new MoveToPositionCommand(_releaseTarget, _token.Token));
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
            SetPlayerGrabbed(false);
            _subject.transform.parent = null;
        }

        private void SetPlayerGrabbed(bool grabbed)
        {
            var player = _subject.GetComponent<AbstractPlayerController>();
            if (player != null)
                player.enabled = !grabbed;

            var susp = _subject.GetComponent<SuspiciousBehaviour>();
            if (susp)
                susp.PlayerGrabbed = grabbed;

        }
    }
}