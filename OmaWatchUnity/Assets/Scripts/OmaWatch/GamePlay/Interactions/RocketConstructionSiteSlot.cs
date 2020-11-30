using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RocketConstructionSiteSlot : MonoBehaviour
    {
        private ScrapPieceConfig _config;

        public ScrapPieceConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                var spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.spriteSortPoint = SpriteSortPoint.Center;
                spriteRenderer.sprite = _config.Sprite;
                spriteRenderer.size = _config.Sprite.rect.size;
            }
        }

        public RocketConfig.RocketConfigElement ElementConfig { get; set; }
        public Vector2 Size => _config != null ? _config.Size : Vector2.zero;

        public int SortingOrder
        {
            get => GetComponent<SpriteRenderer>().sortingOrder;
            set => GetComponent<SpriteRenderer>().sortingOrder = value;
        }
    }
}