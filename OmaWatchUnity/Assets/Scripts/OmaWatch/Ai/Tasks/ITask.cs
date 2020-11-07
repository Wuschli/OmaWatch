using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public enum TaskResult
    {
        Success,
        Cancelled,
        Failed
    }

    public interface ITask
    {
        Task<TaskResult> Run(AgentBehaviour agent);
        void Cancel();
        void OnCompleted(TaskResult result);
    }
}