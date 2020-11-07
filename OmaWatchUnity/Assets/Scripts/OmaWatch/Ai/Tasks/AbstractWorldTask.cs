using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public abstract class AbstractWorldTask : MonoBehaviour, ITask
    {
        public virtual AgentBehaviour.AgentState State => AgentBehaviour.AgentState.Idle;
        public abstract Task<TaskResult> Run(AgentBehaviour agent);
        

        public abstract void Cancel();

        public virtual void OnCompleted(TaskResult result)
        {
        }
        public virtual void Update()
        {
        }
    }
}