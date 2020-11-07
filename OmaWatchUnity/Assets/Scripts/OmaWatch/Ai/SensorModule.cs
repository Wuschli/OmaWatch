using Assets.Scripts.OmaWatch.Ai.Tasks;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class SensorModule : MonoBehaviour
    {
        private AgentBehaviour _agent;

        public Transform target;

        [Range(1, 5)] public float chaseDistance = 1;

        [Range(5, 15)] public float chaseCooldown = 5;

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

            var dist = Vector3.Distance(transform.position, target.position);
            if (dist > chaseDistance)
                return;

            Debug.Log("starting chase");
            _agent.SetTask(new ChaseTask(target, chaseCooldown));
            _currentCooldown = chaseCooldown;
        }
    }
}