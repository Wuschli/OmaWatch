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
                return;

            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(target, chaseTime));
            _agent.EnqueueTask(new ExhaustedTask(restTime));
        }
    }
}