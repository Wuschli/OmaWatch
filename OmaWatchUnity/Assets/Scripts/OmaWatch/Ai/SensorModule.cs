using System;
using Assets.Scripts.OmaWatch.Ai.Tasks;
using Assets.Scripts.OmaWatch.Player;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class SensorModule : MonoBehaviour
    {
        private AgentBehaviour _agent;

        public SuspiciousBehaviour target;

        public float chaseDistance = 1;
        public float chaseTime = 5;
        public float restTime = 1;
        public float agggroTime = 1;

        private float _currentAggro = 0;

        public float Aggro => _currentAggro / agggroTime;

        public void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        public void Update()
        {
            if (_agent.CurrentState != AgentBehaviour.AgentState.Idle)
                return;

            if (!target.IsSuspicious)
                return;

            var dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist > chaseDistance)
            {
                _currentAggro = Math.Max(_currentAggro - Time.deltaTime, 0);
                return;
            }

            _currentAggro = Math.Min(_currentAggro + Time.deltaTime, agggroTime);

            if (_currentAggro < agggroTime)
                return;


            Debug.DrawLine(transform.position, target.transform.position, Color.red, 10);
            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(target, chaseTime));
            _agent.EnqueueTask(new ExhaustedTask(restTime));
            _currentAggro = 0;
        }
    }
}