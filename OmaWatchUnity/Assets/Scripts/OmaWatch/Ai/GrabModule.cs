﻿using Assets.Scripts.OmaWatch.Ai.Tasks;
using Assets.Scripts.OmaWatch.Player;
using Assets.Scripts.OmaWatch.World;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GrabModule : MonoBehaviour
    {
        private AgentBehaviour _agent;


        [FormerlySerializedAs("releasePosition")]
        public Transform ReleasePosition;

        [FormerlySerializedAs("grabDistance")]
        public float GrabDistance = 0.25f;

        private SuspiciousBehaviour GrabTarget => AICoordinator.Instance.SuspiciousBehaviour;

        private void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        private void Update()
        {
            if (_agent.CurrentState != AgentBehaviour.AgentState.Chase)
                return;

            if (!GrabTarget.IsSuspicious)
                return;

            if(Vector3.Distance(transform.position, GrabTarget.transform.position) > GrabDistance)
                return;

            _agent.SetTask(new GrabTask(GrabTarget.gameObject, ReleasePosition));
        }
    }
}