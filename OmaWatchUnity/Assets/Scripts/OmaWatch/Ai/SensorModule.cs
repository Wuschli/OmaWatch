﻿using System;
using System.Linq;
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

        [FormerlySerializedAs("chaseDistance")]
        public float ChaseDistance = 1;

        [FormerlySerializedAs("restTime")]
        public float RestTime = 1;

        [FormerlySerializedAs("agggroTime")]
        public float AggroTime = 1;

        private float _currentAggro = 0;

        public float Aggro => _currentAggro / AggroTime;
        public bool AggroIncreasing { get; private set; }

        public void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        public void Update()
        {
            if (_agent.CurrentState != AgentBehaviour.AgentState.Idle)
                return;

            var dist = Vector3.Distance(transform.position, Target.transform.position);
            if (!Target.IsSuspicious || dist > ChaseDistance || !CanSeeTarget())
            {
                _currentAggro = Math.Max(_currentAggro - Time.deltaTime, 0);
                AggroIncreasing = false;
                return;
            }

            _currentAggro = Math.Min(_currentAggro + Time.deltaTime, AggroTime);
            AggroIncreasing = true;

            if (_currentAggro < AggroTime)
                return;


            //Debug.DrawLine(transform.position, Target.transform.position, Color.red, 10);
            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(Target));
            _agent.EnqueueTask(new ExhaustedTask(RestTime));
            _currentAggro = 0;
        }

        public bool CanSeeTarget()
        {
            var distance = Vector3.Distance(transform.position, Target.transform.position);
            var hit = Physics2D.Raycast(transform.position, (Target.transform.position - transform.position).normalized, distance, LayerMask.GetMask("Walls", "Player"));

            if (hit.transform == Target.transform)
                return true;

            return false;
        }
    }
}