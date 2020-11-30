using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Util
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t => Debug.LogException(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}