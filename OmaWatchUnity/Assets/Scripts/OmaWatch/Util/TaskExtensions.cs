using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Util
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t => Debug.LogException(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}