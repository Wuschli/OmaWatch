using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractPickUpItem : AbstractButtonInteraction
    {
        protected abstract Task<bool> PickUpAsync(AbstractPlayerController player);

        public override async Task<bool> InteractAsync(AbstractPlayerController player)
        {
            if (await PickUpAsync(player))
            {
                Destroy(gameObject);
                return true;
            }

            return false;
        }
    }
}