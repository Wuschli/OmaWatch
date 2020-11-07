using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public abstract class AbstractTask : MonoBehaviour
    {
        public enum TaskResult
        {
            Success,
            Cancelled,
            Failed
        }
        
        public abstract Task<TaskResult> Run(AgentBehaviour agent);
        public abstract void Cancel();
    }
}