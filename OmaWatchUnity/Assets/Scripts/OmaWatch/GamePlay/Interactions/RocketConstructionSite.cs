using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public class RocketConstructionSite : AbstractTouchInteraction
    {
        public RocketConfig Config;
        public CinemachineVirtualCamera VirtualCamera;

        private RocketConstructionSiteSlot[] _slots;

        public RocketConstructionSiteSlot NextSlot => _slots.FirstOrDefault(slot => slot.Config == null);
        public bool AllSlotsFilled => _slots.All(slot => slot.Config != null);

        public bool CanAddScrap(ScrapPieceConfig scrapPiece)
        {
            return NextSlot != null && scrapPiece != null && scrapPiece.Type == NextSlot.ElementConfig.Type;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _slots = Config.Elements.Select(CreateSlotForConfigElement).ToArray();
            LevelCoordinator.Instance.ConstructionSite = this;
        }

        protected override async Task InteractAsync(AbstractPlayerController player)
        {
            while (CanAddScrap(player.ScrapTrail.NextScrapPiece))
            {
                var config = player.ScrapTrail.TakeScrap();
                var slot = _slots.First(s => s.Config == null);
                slot.Config = config;
                UpdateSlotPositions();
                if (LevelCoordinator.Instance.CheckWinningCondition())
                    return;
                await Task.Yield();
            }
        }

        private void UpdateSlotPositions()
        {
            var previousHeight = 0f;
            var previousPosition = Vector3.zero;
            foreach (var slot in _slots)
            {
                slot.transform.localPosition = previousPosition + new Vector3(0, previousHeight, 0);
                previousPosition = slot.transform.localPosition;
                previousHeight = slot.Size.y;
            }

            var lastFilledSlot = _slots.LastOrDefault(slot => slot.Config != null);
            if (lastFilledSlot != null)
                VirtualCamera.Follow = lastFilledSlot.transform;
        }

        private RocketConstructionSiteSlot CreateSlotForConfigElement(RocketConfig.RocketConfigElement e, int layerOrder = 0)
        {
            var go = new GameObject("Slot");
            go.transform.SetParent(transform, false);
            var slot = go.AddComponent<RocketConstructionSiteSlot>();
            slot.ElementConfig = e;
            slot.SortingOrder = layerOrder;
            return slot;
        }
    }
}