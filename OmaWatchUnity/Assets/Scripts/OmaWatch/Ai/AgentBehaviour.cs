using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Ai.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBehaviour : MonoBehaviour
    {
        public AbstractWorldTask defaultTask;

        private readonly Queue<ITask> _taskQueue = new Queue<ITask>();
        public ITask CurrentTask { get; private set; }

        public void Awake()
        {
            var nav = GetComponent<NavMeshAgent>();
            nav.updateRotation = false;
            nav.updateUpAxis = false;
        }

        public void Update()
        {
            if (CurrentTask != null)
                return;
            if (_taskQueue.Count > 0)
                RunTask(_taskQueue.Dequeue());

            RunTask(defaultTask);
        }

        public void SetTask(ITask task)
        {
            _taskQueue.Clear();
            RunTask(task);
        }

        public void EnqueueTask(ITask task)
        {
            _taskQueue.Enqueue(task);
        }

        private async void RunTask(ITask task)
        {
            CurrentTask?.Cancel();

            CurrentTask = task;

            Debug.Log($"[{Time.frameCount}] START {task.GetType().Name}");
            var result = await task.Run(this);
            Debug.Log($"[{Time.frameCount}] COMPLETE {task.GetType().Name} ({result})");

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