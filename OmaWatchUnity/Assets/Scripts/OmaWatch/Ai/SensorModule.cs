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

        public float chaseCooldown = 5;

        private float _currentCooldown;

        public void Awake()
        {
            _agent = GetComponent<AgentBehaviour>();
        }

        public void Update()
        {
            if (_agent.CurrentTask is ChaseTask || _agent.CurrentTask is GrabTask)
                return;

            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
                return;
            }

            if (!target.IsSuspicious)
                return;

            var dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist > chaseDistance)
                return;

            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(target.transform, chaseCooldown));
            _currentCooldown = chaseCooldown;
        }
    }
}