using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyStateManager : MonoBehaviour
{
    #region ChaseValues

    [Header("Chase Values")] 
    [SerializeField] private float lastPOSRefreshRate = 1f;
    [SerializeField] private float patrolSpeed = 4f;
    [SerializeField] private float stalkingSpeed = 5f;
    [SerializeField] private float chaseSpeed = 8f;
    #endregion
    
    
    public enum EnemyState{
        PATROL,
        STALKING,
        CHASING,
        TRACKING
    }

    public EnemyState currentState;
    public event System.Action OnArrival;
    
    
    
    [HideInInspector]public Transform[] patrolNodes;
    [SerializeField]private GameObject nodesParent;
    [HideInInspector] public bool isSeeingLight;
    [HideInInspector] public bool isSeeingPlayer;
    
    [SerializeField]private AnimatorController FSM;

    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private EnemyBehaviour enemyBehaviour;
    public EnemyBehaviour EnemyBehaviour
    {
        get { return enemyBehaviour;}
        
    }
    
    
    private Transform lastSeenLocation;
    private float timeSinceLastRefresh;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        anim = GetComponent<Animator>();
        patrolNodes = new Transform[nodesParent.transform.childCount];
        timeSinceLastRefresh = lastPOSRefreshRate;
        for (int i = 0; i < patrolNodes.Length; i++)
        {
            patrolNodes[i] = nodesParent.transform.GetChild(i).transform;
        }
        OnArrival += HandleArrival;
        anim.runtimeAnimatorController = FSM;
    }

    
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.STALKING:
                //Stalk();
                break;
            case EnemyState.CHASING:
                //Chase();
                break;
            case EnemyState.TRACKING:
                //Track();
                break;
        }
        CheckArrival();
        UpdateAnimatorParams();
    }

    void CheckArrival()
    {
        
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (OnArrival != null)
            {
                OnArrival.Invoke();
            }
        }
    }

    void UpdateAnimatorParams()
    {
        anim.SetBool("isSeeingLight", isSeeingLight);
        anim.SetBool("isSeeingPlayer", isSeeingPlayer);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private int nodeIndex = 0;
    public void Patrol()
    {
        navMeshAgent.acceleration = patrolSpeed;
        //Debug.Log("isPatrolling");
        
        if (patrolNodes == null || patrolNodes.Length == 0) return;
        Vector3 target = patrolNodes[nodeIndex].position;
        target.y = transform.position.y;
        MoveTowards(target);
        
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            nodeIndex = (nodeIndex + 1) % patrolNodes.Length;
        }
        GoToPlayer();
    }

    public void Stalk()
    {
        
        navMeshAgent.acceleration = stalkingSpeed;
        GoToPlayer();
        MoveTowards(lastSeenLocation.position);
    }

    public void Chase()
    {
        navMeshAgent.acceleration = chaseSpeed;
        GoToPlayer();
        MoveTowards(lastSeenLocation.position);
    }

    public void Track()
    {
        MoveTowards(lastSeenLocation.position);
    }

    public void MoveTowards(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        
    }

    void HandleArrival()
    {
        
        
    }
    

    SphereCollider LookForPlayer()
    {
        SphereCollider hitCollider = null;
        Dictionary<Transform, RaycastHit[]> bigDick = new();
        bigDick = enemyBehaviour.ShootRaycasts();
        if (enemyBehaviour.DirectPlayerSight)
        {
            foreach (KeyValuePair<Transform, RaycastHit[]> smallDick in bigDick)
            {
                foreach (RaycastHit hitDick in smallDick.Value)
                {
                    if (hitDick.collider.CompareTag("Player"))
                    {
                        Debug.Log("Chasing player");
                        hitCollider = hitDick.collider.gameObject.GetComponent<SphereCollider>();
                    }
                    else if (hitDick.collider.CompareTag("LightArea"))
                    {
                        Debug.Log("Chasing light");
                        enemyBehaviour.CheckPlayerExtremities();
                        hitCollider = hitDick.collider.gameObject.GetComponent<SphereCollider>();
                    }
                }
            }
        }
        else
        {
            //Debug.Log("Can't see player");
        }
        

        return hitCollider;
    }

    void GoToPlayer()
    {
        SphereCollider hitSphere = LookForPlayer();
        
        if (hitSphere != null)
        {
            if (timeSinceLastRefresh <= lastPOSRefreshRate)
            {
                timeSinceLastRefresh += Time.deltaTime;
            }
            else
            {
                lastSeenLocation = hitSphere.transform;
                timeSinceLastRefresh = 0;
            }
            
            if (hitSphere.CompareTag("Player"))
            {
                isSeeingPlayer = true;
            }
            else if (hitSphere.CompareTag("LightArea"))
            {
                isSeeingLight = true;
            }
            
        }
        else
        {
            isSeeingLight = false;
            isSeeingPlayer = false;
        }
    }

    private void OnDestroy()
    {
        OnArrival -= HandleArrival;
    }
}
