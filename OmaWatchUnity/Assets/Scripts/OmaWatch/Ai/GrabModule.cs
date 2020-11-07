using System;
using Assets.Scripts.OmaWatch.Ai.Tasks;
using Assets.Scripts.OmaWatch.Player;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GrabModule : MonoBehaviour
    {
        private AgentBehaviour _agent;

        public SuspiciousBehaviour grabTarget;
        public Transform releasePosition;

        public float grabDistance = 0.25f;
        public float grabCooldown = 5;
        private float _currentCooldown = 0;

        private void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        private void Update()
        {
            if (_agent.CurrentTask is GrabTask)
                return;

            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
                return;
            }

            if(!grabTarget.IsSuspicious)
                return;

            var distance = Vector3.Distance(transform.position, grabTarget.transform.position);
            if (distance > grabDistance)
                return;

            _currentCooldown = grabCooldown;
            _agent.SetTask(new GrabTask(grabTarget.gameObject, releasePosition));
        }
    }
}