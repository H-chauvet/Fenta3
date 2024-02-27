using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    
        public string targetTag = "YourTargetTag"; 
        public float raycastDistance = 10f;
        [SerializeField] private float forwardOffset = 5f;
        [SerializeField] private float sidesOffset = 7.5f;
        [SerializeField] private float backOffset = 2.5f;
        
        
        private NavMeshAgent navMeshAgent;
        //I know there's probably a way better way of doing this, but this will suffice for now
        private Transform leftForwardExtremity;
        private Transform rightForwardExtremity;
        private Transform leftSideExtremity;
        private Transform rightSideExtremity;
        private Transform leftBackExtremity;
        private Transform rightBackExtremity;
        private Transform[] extremities;
        
        void Start()
        {
            InitializeExtremities();
            navMeshAgent = GetComponent<NavMeshAgent>();
           
        }

        void InitializeExtremities()
        {
            extremities = new Transform[]
            {
                leftForwardExtremity, rightForwardExtremity,
                leftSideExtremity, rightSideExtremity,
                leftBackExtremity, rightBackExtremity
            };

            foreach (Transform extremity in extremities)
            {
                Transform newExtremity = new GameObject(extremity.name).transform;
                extremity.SetParent(transform);
            }
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

        void UpdateExtremities()
        {
            
        }
        
        void ShootRaycasts()
        {
            
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
