using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public abstract class AbstractWorldTask : MonoBehaviour, ITask
    {
        public abstract Task<TaskResult> Run(AgentBehaviour agent);

        public abstract void Cancel();
    }
}