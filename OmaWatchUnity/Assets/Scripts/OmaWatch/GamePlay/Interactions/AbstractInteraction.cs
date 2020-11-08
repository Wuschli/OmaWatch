using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractInteraction : MonoBehaviour
    {
        protected abstract Task InteractAsync(AbstractPlayerController player);
    }
}