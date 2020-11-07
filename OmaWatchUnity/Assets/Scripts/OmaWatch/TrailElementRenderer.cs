using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TrailElementRenderer : MonoBehaviour
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