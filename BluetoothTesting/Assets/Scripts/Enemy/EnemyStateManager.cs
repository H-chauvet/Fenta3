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
    [SerializeField] private float trackLingerDelay = 3f;
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
    
    
    public Transform currentDestination;
    [HideInInspector]public Transform[] patrolNodes;
    [SerializeField]private GameObject nodesParent;
    [HideInInspector] public bool isSeeingLight;
    [HideInInspector] public bool isSeeingPlayer;
    
    [SerializeField]private AnimatorController FSM;

    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private EnemyBehaviour enemyBehaviour;
    
    
    private Transform lastSeenLocation;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        anim = GetComponent<Animator>();
        patrolNodes = new Transform[nodesParent.transform.childCount];
        for (int i = 0; i < patrolNodes.Length; i++)
        {
            patrolNodes[i] = nodesParent.transform.GetChild(i).transform;
        }
        
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
        
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
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
    
    public void Patrol()
    {
        navMeshAgent.acceleration = patrolSpeed;
    }

    public void Stalk()
    {
        navMeshAgent.acceleration = stalkingSpeed;
        MoveTowards(lastSeenLocation.position);
    }

    public void Chase()
    {
        navMeshAgent.acceleration = chaseSpeed;
        MoveTowards(lastSeenLocation.position);
    }

    public void Track()
    {
        StartCoroutine(TrackingLinger());
    }

    public void MoveTowards(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        OnArrival += HandleArrival;
    }

    void HandleArrival()
    {
        Debug.Log("Has arrived");
        switch (currentState)
        {
            case EnemyState.PATROL:
                
                break;
            case EnemyState.STALKING:
                currentDestination = null;
                isSeeingLight = false;
                break;
            case EnemyState.CHASING:
                currentDestination = null;
                isSeeingPlayer = false;
                break;
            case EnemyState.TRACKING:
                //StartCoroutine(TrackingLinger());
                break;
        }   
        OnArrival -= HandleArrival;
    }
    
    IEnumerator TrackingLinger()
    {
        yield return new WaitForSeconds(trackLingerDelay);
        //currentState = EnemyState.PATROL;
    }
    
    public IEnumerator UpdateLastSeenPosition()
    {
        while (true)
        {
            lastSeenLocation = currentDestination;
            yield return new WaitForSeconds(lastPOSRefreshRate);
            //MoveTowards(lastSeenLocation.position);
        }
    }
}
