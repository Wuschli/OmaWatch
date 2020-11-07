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
                GetComponent<SpriteRenderer>().sprite = _config.Sprite;
            }
        }
    }
}