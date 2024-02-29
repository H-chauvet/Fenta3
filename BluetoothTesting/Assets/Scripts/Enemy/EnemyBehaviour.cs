using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateManager))]
public class EnemyBehaviour : MonoBehaviour
{

        
    #region ExtremityOffsets
    [Header("Extremity Offsets")] 
    [SerializeField] private float forwardDistance = 4f;
    [SerializeField] private float backDistance = 2f;
    [SerializeField] private float forwardOffset = 5f;
    [SerializeField] private float sidesOffset = 7.5f;
    [SerializeField] private float backOffset = 2.5f;
    #endregion
        
    private EnemyStateManager enemyStateManager;
    private EnemyStateManager.EnemyState _currentState;
    //public bool isTrackingLight;
    //public bool isTrackingPlayer;
        
        
    #region Extremities
    //I know there's probably a way better way of doing this, but this will suffice for now
    private Transform leftForwardExtremity;
    private Transform rightForwardExtremity;
    private Transform leftSideExtremity;
    private Transform rightSideExtremity;
    private Transform leftBackExtremity;
    private Transform rightBackExtremity;
    private Transform[] extremities;
    #endregion
        
    //This should be moved to EnemyStateManager
    #region ChaseValues

    [Header("Chase Values")] 
    [SerializeField] private float lastPOSRefreshRate = 1f;
    [SerializeField] private float trackLingerDelay = 3f;
    [SerializeField] private float patrolSpeed = 4f;
    [SerializeField] private float trackingSpeed = 5f;
    [SerializeField] private float chaseSpeed = 8f;
    #endregion
        
    void Start()
    {
        InitializeExtremities();
        //_currentState = enemyStateManager.currentState;
        enemyStateManager = GetComponent<EnemyStateManager>();
    }

    void InitializeExtremities()
    {
        extremities = new Transform[6];
        //Behold true horror. I'm truly sorry
        leftForwardExtremity = new GameObject("leftForwardExtremity").transform;
        leftForwardExtremity.SetParent(transform);
            
        rightForwardExtremity = new GameObject("RightForwardExtremity").transform;
        rightForwardExtremity.SetParent(transform);
            
        leftSideExtremity = new GameObject("LeftSideExtremity").transform;
        leftSideExtremity.SetParent(transform);
            
        rightSideExtremity = new GameObject("RightSideExtremity").transform;
        rightSideExtremity.SetParent(transform);
            
        leftBackExtremity = new GameObject("LeftBackExtremity").transform;
        leftBackExtremity.SetParent(transform);
            
        rightBackExtremity = new GameObject("RightBackExtremity").transform;
        rightBackExtremity.SetParent(transform);
            
        extremities[0] = leftForwardExtremity;
        extremities[1] = rightForwardExtremity;
        extremities[2] = leftSideExtremity;
        extremities[3] = rightSideExtremity;
        extremities[4] = leftBackExtremity;
        extremities[5] = rightBackExtremity;
            
    }

    void Update()
    {
        UpdateExtremities();
        ShootRaycasts();
        //enemyStateManager.currentState = _currentState;
        //CheckArrival();
    }

    void UpdateExtremities()
    {
        leftForwardExtremity.position = transform.position +
                                        transform.forward * forwardDistance + (-1) * transform.right * forwardOffset;
        rightForwardExtremity.position = transform.position +
                                         transform.forward * forwardDistance +  transform.right * forwardOffset;
            
        leftSideExtremity.position = transform.position + (-1) * transform.right * sidesOffset;
        rightSideExtremity.position = transform.position + transform.right * sidesOffset;
            
        leftBackExtremity.position = transform.position +
                                     (-1) * transform.forward * backDistance + (-1) * transform.right * backOffset;
        rightBackExtremity.position = transform.position +
                                      (-1) * transform.forward * backDistance + transform.right * backOffset;
    }
        
    void ShootRaycasts()
    {
        foreach (Transform extremity in extremities)
        {
            foreach (Transform oExtremity in extremities)
            {
                if (extremity == oExtremity) continue;

                Vector3 direction = (oExtremity.position - extremity.position).normalized;
                float distance = Vector3.Distance(extremity.position, oExtremity.position);

                RaycastHit[] hits = Physics.RaycastAll(extremity.position, direction, distance);

                
                
                foreach (RaycastHit hit in hits)
                {
                        
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Targeting player");
                        enemyStateManager.currentDestination = hit.collider.gameObject.transform;
                        StartCoroutine(enemyStateManager.UpdateLastSeenPosition());
                        enemyStateManager.isSeeingPlayer = true;
                        //isTrackingPlayer = true;

                    }
                    else if (hit.collider.CompareTag("LightArea"))
                    {
                        Debug.Log("Targeting light");
                        enemyStateManager.currentDestination = hit.collider.gameObject.transform;
                        StartCoroutine(enemyStateManager.UpdateLastSeenPosition());
                        enemyStateManager.isSeeingLight = true;
                        //isTrackingLight = true;
                    }
                            
                   

                        
                }
            }
        }
    }

    
    
    void OnDrawGizmos()
    {
        if (extremities == null) return;
        Gizmos.color = Color.magenta;
        foreach (Transform extremity in extremities)
        {
            foreach (Transform oExtremity in extremities)
            {
                //if (extremity == oExtremity) continue;
                Gizmos.DrawLine(extremity.position, oExtremity.position);
            }
        }
    }
}