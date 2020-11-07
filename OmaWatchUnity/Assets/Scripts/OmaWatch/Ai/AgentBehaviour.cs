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

        public void Start()
        {
            defaultTask.Run(this);
        }

        public void Update()
        {

        }


        public Task<AbstractCommand.CommandResult> ExecuteCommand(AbstractCommand command)
        {
            return command.Execute(this);
        }

    }
}