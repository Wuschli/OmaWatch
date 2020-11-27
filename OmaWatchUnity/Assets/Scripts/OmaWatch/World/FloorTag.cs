using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.OmaWatch.World
{
    [RequireComponent(typeof(Tilemap))]
    public class FloorTag : MonoBehaviour
    {
        public Tilemap Tilemap => GetComponent<Tilemap>();
    }
}