using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractPickUpItem : AbstractTouchInteraction
    {
        protected abstract Task<bool> PickUpAsync(PlayerController player);

        protected override async Task InteractAsync(PlayerController player)
        {
            if (await PickUpAsync(player))
                Destroy(gameObject);
        }
    }
}