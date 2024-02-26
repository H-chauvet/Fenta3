using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    
    public string targetTag = "YourTargetTag"; // Specify the target tag
        public float raycastDistance = 10f;
        private NavMeshAgent navMeshAgent;
    
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    
        void Update()
        {
            // Shoot a raycast forward
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                // Check if the hit object has the specified tag
                if (hit.collider.CompareTag(targetTag))
                {
                    // Move towards the target
                    MoveTowards(hit.collider.gameObject.transform.position);
                }
            }
        }
    
        void MoveTowards(Vector3 targetPosition)
        {
            // Set the destination for the NavMeshAgent to move towards
            navMeshAgent.SetDestination(targetPosition);
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
        }
}
