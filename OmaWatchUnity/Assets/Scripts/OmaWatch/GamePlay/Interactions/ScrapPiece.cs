using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public class ScrapPiece : AbstractPickUpItem
    {
        protected override async Task PickUpAsync(PlayerController player)
        {
            await Task.Yield();
            Debug.Log("SCRAP!");
        }
    }
}