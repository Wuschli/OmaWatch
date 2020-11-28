using Assets.Scripts.OmaWatch.World;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Player
{
    public class SuspiciousBehaviour : MonoBehaviour
    {
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

            if (!WorldRoot.Instance.IsTileSafe(transform.position))
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