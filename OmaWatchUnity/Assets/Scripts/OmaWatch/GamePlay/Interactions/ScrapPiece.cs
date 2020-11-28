using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class ScrapPiece : AbstractPickUpItem
    {
        [FormerlySerializedAs("Config")]
        [SerializeField]
        private ScrapPieceConfig _config;

        public ScrapPieceConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                UpdateSprite();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_config == null)
                return;
            UpdateSprite();
        }

        protected override async Task<bool> PickUpAsync(AbstractPlayerController player)
        {
            if (!player.enabled)
                return false;

            await Task.Yield();
            Debug.Log("SCRAP!");
            if (player.ScrapTrail != null)
                return player.ScrapTrail.TryAddElement(_config);
            return false;
        }

        private void UpdateSprite()
        {
            var sr = GetComponent<SpriteRenderer>();
            sr.sprite = _config.Sprite;
            sr.transform.localScale = Vector3.one / 2;
        }
    }
}