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
        public AbstractTask defaultTask;


        private readonly Queue<AbstractTask> _taskQueue = new Queue<AbstractTask>();
        private AbstractTask _currentTask;

        public void Awake()
        {
            var nav = GetComponent<NavMeshAgent>();
            nav.updateRotation = false;
            nav.updateUpAxis = false;
        }

        public void Update()
        {
            if(_currentTask != null)
                return;
            if(_taskQueue.Count > 0)
                RunTask(_taskQueue.Dequeue());

            RunTask(defaultTask);
        }

        private async void RunTask(AbstractTask task)
        {
            if(_currentTask != null)
                _currentTask.Cancel();

            _currentTask = task;
            await task.Run(this);
            _currentTask = null;
        }

        private void OnApplicationQuit()
        {
            if(_currentTask != null)
                _currentTask.Cancel();
        }

        public Task<AbstractCommand.CommandResult> ExecuteCommand(AbstractCommand command)
        {
            return command.Execute(this);
        }

    }
}