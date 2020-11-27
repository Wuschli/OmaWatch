using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch
{
    public class NavMeshConstrainer : MonoBehaviour
    {
        private void Update()
        {
            if (NavMesh.SamplePosition(transform.position, out var hit, 1.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
        }
    }
}