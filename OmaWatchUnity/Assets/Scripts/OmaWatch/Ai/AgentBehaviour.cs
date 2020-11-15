using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Ai.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class AgentBehaviour : MonoBehaviour
    {
        public enum AgentState
        {
            Idle,
            Chase,
            Grab,
            Exhausted
        }

        public AbstractWorldTask defaultTask;
        public SpriteRenderer chaseIndicator;

        private readonly Queue<ITask> _taskQueue = new Queue<ITask>();
        public ITask CurrentTask { get; private set; }
        public AgentState CurrentState { get; private set; } = AgentState.Idle;

        public TileNavMovementController Nav => GetComponent<TileNavMovementController>();

        

        public void Update()
        {
            if (CurrentTask != null)
            {
                CurrentTask.Update();
                return;
            }

            if (_taskQueue.Count > 0)
            {
                RunTask(_taskQueue.Dequeue());
                return;
            }

            RunTask(defaultTask);
        }

        public void SetTask(ITask task)
        {
            _taskQueue.Clear();
            RunTask(task);
        }

        public void SetState(AgentState state)
        {
            if(CurrentState == state)
                return;

            switch (CurrentState)
            {
                case AgentState.Chase:
                    chaseIndicator.enabled = false;
                    Nav.Speed = 1f;
                    break;
            }

            CurrentState = state;

            switch (CurrentState)
            {
                case AgentState.Chase:
                    chaseIndicator.enabled = true;
                    Nav.Speed = 1.1f;
                    break;
            }
        }

        public void EnqueueTask(ITask task)
        {
            _taskQueue.Enqueue(task);
        }

        private async void RunTask(ITask task)
        {
            CurrentTask?.Cancel();

            CurrentTask = task;
            SetState(task.State);

            Debug.Log($"[{Time.frameCount}] START {task.GetType().Name}");
            var result = await task.Run(this);
            Debug.Log($"[{Time.frameCount}] COMPLETE {task.GetType().Name} ({result})");
            task.OnCompleted(result);

            if (CurrentTask == task)
                CurrentTask = null;
        }

        private void OnApplicationQuit()
        {
            CurrentTask?.Cancel();
        }

        public Task<AbstractCommand.CommandResult> ExecuteCommand(AbstractCommand command)
        {
            return command.Execute(this);
        }
    }
}