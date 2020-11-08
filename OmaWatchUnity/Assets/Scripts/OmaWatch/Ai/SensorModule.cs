﻿using System;
using Assets.Scripts.OmaWatch.Ai.Tasks;
using Assets.Scripts.OmaWatch.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class SensorModule : MonoBehaviour
    {
        private AgentBehaviour _agent;

        private SuspiciousBehaviour Target => AICoordinator.Instance.SuspiciousBehaviour;

        [FormerlySerializedAs("chaseDistance")] public float ChaseDistance = 1;
        [FormerlySerializedAs("chaseTime")] public float ChaseTime = 5;
        [FormerlySerializedAs("restTime")] public float RestTime = 1;
        [FormerlySerializedAs("agggroTime")] public float AggroTime = 1;

        private float _currentAggro = 0;

        public float Aggro => _currentAggro / AggroTime;

        public void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        public void Update()
        {
            if (_agent.CurrentState != AgentBehaviour.AgentState.Idle)
                return;

            if (!Target.IsSuspicious)
                return;

            var dist = Vector3.Distance(transform.position, Target.transform.position);
            if (dist > ChaseDistance)
            {
                _currentAggro = Math.Max(_currentAggro - Time.deltaTime, 0);
                return;
            }

            _currentAggro = Math.Min(_currentAggro + Time.deltaTime, AggroTime);

            if (_currentAggro < AggroTime)
                return;


            Debug.DrawLine(transform.position, Target.transform.position, Color.red, 10);
            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(Target, ChaseTime));
            _agent.EnqueueTask(new ExhaustedTask(RestTime));
            _currentAggro = 0;
        }
    }
}