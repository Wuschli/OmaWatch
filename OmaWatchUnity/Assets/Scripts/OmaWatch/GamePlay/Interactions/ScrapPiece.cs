using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScrapPiece : AbstractPickUpItem
    {
        public ScrapPieceConfig Config;

        protected override void OnEnable()
        {
            base.OnEnable();
            GetComponent<SpriteRenderer>().sprite = Config.Sprite;
        }

        protected override async Task<bool> PickUpAsync(AbstractPlayerController player)
        {
            if (!player.enabled)
                return false;

            await Task.Yield();
            Debug.Log("SCRAP!");
            if (player.ScrapTrail != null)
                return player.ScrapTrail.TryAddElement(Config);
            return false;
        }
    }
}