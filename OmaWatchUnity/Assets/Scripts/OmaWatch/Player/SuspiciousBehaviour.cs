using Assets.Scripts.OmaWatch.World;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.OmaWatch.Player
{
    public class SuspiciousBehaviour : MonoBehaviour
    {
        [FormerlySerializedAs("safeTilemap")]
        public Tilemap SafeTilemap;
        private ScrapTrail _trail;

        public bool IsSuspicious { get; private set; }
        public bool PlayerGrabbed { get; set; }

        public GameObject DEBUG_TileIndicator;

        private void Awake()
        {
            _trail = GetComponentInChildren<ScrapTrail>();
        }

        private void Update()
        {
            if (DEBUG_TileIndicator != null)
                DEBUG_TileIndicator.transform.position = WorldRoot.Instance.ClampToTile(transform.position);

            if (PlayerGrabbed)
            {
                SetSuspicious(false);
                return;
            }

            if (_trail != null && _trail.HasScrap)
            {
                SetSuspicious(true);
                return;
            }

            var position = transform.position;
            var local = SafeTilemap.WorldToCell(position);
            local.z = 0;

            var tile = SafeTilemap.GetTile(local);
            if (tile == null)
            {
                SetSuspicious(true);
                return;
            }

            SetSuspicious(false);
        }

        private void SetSuspicious(bool setSuspicious)
        {
            if (setSuspicious != IsSuspicious)
            {
                IsSuspicious = setSuspicious;
                Debug.Log($"suspicious changed: {setSuspicious}");
            }
        }
    }
}