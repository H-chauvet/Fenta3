using System;
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

    private ConeRaycaster playerExtremities;
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
        
    public Dictionary<Transform, RaycastHit[]> ShootRaycasts()
    {
        Dictionary<Transform, RaycastHit[]> dick = new Dictionary<Transform, RaycastHit[]>();
        foreach (Transform extremity in extremities)
        {
            foreach (Transform oExtremity in extremities)
            {
                if (extremity == oExtremity) continue;

                Vector3 direction = (oExtremity.position - extremity.position).normalized;
                float distance = Vector3.Distance(extremity.position, oExtremity.position);

                RaycastHit[] hits = Physics.RaycastAll(extremity.position, direction, distance);
                if(dick.ContainsKey(extremity)) continue;
                dick.Add(extremity, hits);
                
            }
        }

        return dick;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    // public void CheckPlayerExtremities()
    // {
    //     playerExtremities = GameManager.Instance.light.GetComponent<ConeRaycaster>();
    //     Transform playerMidLength = playerExtremities.midLength;
    //     Transform playerCenterBase = playerExtremities.centerBase;
    //     Transform playerRightExtremity = playerExtremities.basePoints[2];
    //     Transform playerLeftExtremity = playerExtremities.basePoints[3];
    //     
    //     Vector3 midDirection = (playerMidLength.position - transform.position).normalized;
    //     float midDistance = Vector3.Distance(transform.position, playerMidLength.position);
    //
    //     RaycastHit[] midHits = Physics.RaycastAll(transform.position, midDirection, midDistance);
    //     
    //     if (midHits.Length != 0 && midHits[0].collider.CompareTag("Extremity"))
    //     {
    //         Debug.Log("Seeing midpoint");
    //     }
    // }
    
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