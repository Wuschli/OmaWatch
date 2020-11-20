using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class MoveScript : MonoBehaviour
    {
        public GameObject moveTarget;

        // Start is called before the first frame update
        void Start()
        {
            var comp = GetComponent<NavMeshAgent>();
            comp.updateRotation = false;
            comp.updateUpAxis = false;
            //comp.Move(moveTarget.transform.position);
            comp.SetDestination(moveTarget.transform.position);
            comp.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
