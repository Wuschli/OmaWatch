using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.Ai.Commands
{
    public abstract class AbstractCommand
    {
        public enum CommandResult
        {
            Success,
            Failed,
            Cancelled
        }

        public abstract Task<CommandResult> Execute(AgentBehaviour agent);
    }
}