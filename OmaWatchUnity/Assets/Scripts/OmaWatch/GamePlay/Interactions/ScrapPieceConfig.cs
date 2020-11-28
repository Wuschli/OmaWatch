using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [CreateAssetMenu(menuName = "Create ScrapPieceConfig", fileName = "ScrapPieceConfig", order = 0)]
    public class ScrapPieceConfig : ScriptableObject
    {
        private const float PixelsPerUnit = 200;

        public RocketPartType Type;
        public Sprite Sprite;
        public Vector2 Size;

        [ContextMenu(nameof(SetSizeToSpriteSize))]
        public void SetSizeToSpriteSize()
        {
            Size = Sprite.rect.size / PixelsPerUnit;
        }
    }
}