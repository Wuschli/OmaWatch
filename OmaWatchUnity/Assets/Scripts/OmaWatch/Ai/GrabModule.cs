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
        
        private void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        private void Update()
        {
            if (_agent.CurrentState != AgentBehaviour.AgentState.Chase)
                return;

            if(!grabTarget.IsSuspicious)
                return;

            var distance = Vector3.Distance(transform.position, grabTarget.transform.position);
            if (distance > grabDistance)
                return;

            _agent.SetTask(new GrabTask(grabTarget.gameObject, releasePosition));
        }
    }
}