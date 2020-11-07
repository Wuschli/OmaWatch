using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractPickUpItem : AbstractTouchInteraction
    {
        protected abstract Task PickUpAsync(PlayerController player);

        protected override async Task InteractAsync(PlayerController player)
        {
            await PickUpAsync(player);
            Destroy(gameObject);
        }
    }
}