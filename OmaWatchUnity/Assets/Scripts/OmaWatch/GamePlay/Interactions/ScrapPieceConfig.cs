using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [CreateAssetMenu(menuName = "Create ScrapPieceConfig", fileName = "ScrapPieceConfig", order = 0)]
    public class ScrapPieceConfig : ScriptableObject
    {
        public RocketPartType Type;
        public Sprite Sprite;
    }
}