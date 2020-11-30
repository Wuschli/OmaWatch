using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractInteraction : MonoBehaviour
    {
        public abstract Task<bool> InteractAsync(AbstractPlayerController player);
    }
}