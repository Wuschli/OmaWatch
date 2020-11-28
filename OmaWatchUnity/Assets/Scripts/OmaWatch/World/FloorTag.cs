using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.OmaWatch.World
{
    [RequireComponent(typeof(Tilemap))]
    public class FloorTag : MonoBehaviour
    {
        public enum FloorType
        {
            Safe,
            Unsafe
        }

        public FloorType Type;
        public Tilemap Tilemap => GetComponent<Tilemap>();
    }
}