using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public class RocketConstructionSite : AbstractTouchInteraction
    {
        public RocketConfig Config;
        public CinemachineVirtualCamera VirtualCamera;
        public Transform SlotsRoot;

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

        public override async Task<bool> InteractAsync(AbstractPlayerController player)
        {
            var anyPieceAdded = false;
            while (CanAddScrap(player.ScrapTrail.NextScrapPiece))
            {
                anyPieceAdded = true;
                var config = player.ScrapTrail.TakeScrap();
                var slot = _slots.First(s => s.Config == null);
                slot.Config = config;
                UpdateSlotPositions();
                if (LevelCoordinator.Instance.CheckWinningCondition())
                    return true;
                await Task.Yield();
            }

            return anyPieceAdded;
        }

        public void RocketLaunch()
        {
            foreach (var slot in _slots)
            {
                slot.SortingOrder += 10;
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
            go.transform.SetParent(SlotsRoot, false);
            var slot = go.AddComponent<RocketConstructionSiteSlot>();
            slot.ElementConfig = e;
            slot.SortingOrder = layerOrder;
            return slot;
        }
    }
}