using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Camera
{
    public class CinemachineColliderActivator : MonoBehaviour
    {
        public int TargetPriority = 20;

        private CinemachineVirtualCamera _virtualCamera;

        protected void OnEnable()
        {
            if (GetComponent<Collider2D>() == null)
                Debug.LogWarning($"{nameof(CinemachineColliderActivator)} needs an attached Collider!");
            _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _virtualCamera.Priority = 0;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<AbstractPlayerController>();
            if (player == null)
                return;
            _virtualCamera.Priority = TargetPriority;
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<AbstractPlayerController>();
            if (player == null)
                return;
            _virtualCamera.Priority = 0;
        }
    }
}