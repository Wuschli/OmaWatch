using System.Linq;
using System.Threading.Tasks;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public class RocketConstructionSite : AbstractTouchInteraction
    {
        private RocketConstructionSiteSlot[] _slots;
        public bool CanAddScrap => _slots.Any(slot => slot.Config == null);

        protected override void OnEnable()
        {
            base.OnEnable();
            _slots = GetComponentsInChildren<RocketConstructionSiteSlot>().ToArray();
        }

        protected override async Task InteractAsync(AbstractPlayerController player)
        {
            while (CanAddScrap && player.ScrapTrail.HasScrap)
            {
                var config = player.ScrapTrail.TakeScrap();
                var slot = _slots.First(s => s.Config == null);
                slot.Config = config;
                await Task.Yield();
            }
        }
    }
}