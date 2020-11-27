using System.Threading.Tasks;

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
        AgentBehaviour.AgentState State { get; }
        Task<TaskResult> Run(AgentBehaviour agent);
        void Update();
        void Cancel();
        void OnCompleted(TaskResult result);
    }
}