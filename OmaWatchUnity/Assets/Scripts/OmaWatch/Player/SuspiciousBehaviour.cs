using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.OmaWatch.Player
{
    public class SuspiciousBehaviour : MonoBehaviour
    {
        public Tilemap safeTilemap;

        public bool IsSuspicious { get; private set; }

        private void Update()
        {
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