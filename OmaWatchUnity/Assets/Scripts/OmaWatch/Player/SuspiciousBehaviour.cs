using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.OmaWatch.Player
{
    public class SuspiciousBehaviour : MonoBehaviour
    {
        public Tilemap safeTilemap;
        private ScrapTrail _trail;

        public bool IsSuspicious { get; private set; }
        public bool PlayerGrabbed { get; set; }

        private void Awake()
        {
            _trail = GetComponentInChildren<ScrapTrail>();
        }

        private void Update()
        {
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
            var local = safeTilemap.WorldToCell(position);
            local.z = 0;

            var tile = safeTilemap.GetTile(local);
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