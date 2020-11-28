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
                var sr = GetComponent<SpriteRenderer>();
                sr.sprite = _config.Sprite;
                sr.transform.localScale = Vector3.one / 2;
            }
        }
    }
}