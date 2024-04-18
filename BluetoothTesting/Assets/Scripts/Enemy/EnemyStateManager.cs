using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

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
    
    #region EnemySounds

    [SerializeField] private AudioClip enemyWalking;
    [SerializeField] private AudioClip enemySight;
    [SerializeField] private AudioClip enemyChasing;

    private AudioSource enemyWalkSource;
    [HideInInspector]public AudioSource enemySightSource;
    [HideInInspector]public AudioSource enemyChaseSource;
    #endregion
    
    private Vector3 lastSeenLocation;
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
        
        
        //Initialize audio
        enemyWalkSource = gameObject.AddComponent<AudioSource>();
        enemyWalkSource.clip = enemyWalking;
        
        enemySightSource = gameObject.AddComponent<AudioSource>();
        enemySightSource.clip = enemySight;
        
        enemyChaseSource = gameObject.AddComponent<AudioSource>();
        enemyChaseSource.clip = enemyChasing;
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
        if(!enemyWalkSource.isPlaying)enemyWalkSource.Play();
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
        MoveTowards(lastSeenLocation);
    }

    public void Chase()
    {
        navMeshAgent.acceleration = chaseSpeed;
        GoToPlayer();
        MoveTowards(lastSeenLocation);
    }

    public void Track()
    {
        //Debug.Log("Last Seen Location is: " + lastSeenLocation.position);
        //Debug.Log("Player location is: " + GameManager.Instance.player.transform.position);
        MoveTowards(lastSeenLocation);
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
                        //Debug.Log("Chasing player");
                        hitCollider = hitDick.collider.gameObject.GetComponent<SphereCollider>();
                        isSeeingPlayer = true;
                    }
                    else if (hitDick.collider.CompareTag("LightArea"))
                    {
                        //Debug.Log("Chasing light");
                        hitCollider = hitDick.collider.gameObject.GetComponent<SphereCollider>();
                        isSeeingLight = true;
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
 #if UNITY_EDITOR || DEBUG
    private void OnGUI()
    {
        //if (lastSeenLocation is null) return;
        GUI.Label(new Rect(20, 20, 200, 50), "Last Seen Location" + lastSeenLocation.ToString());
        GUI.Label(new Rect(20, 70, 200, 50), "Player Location" + GameManager.Instance.player.transform.position.ToString());
    }
    #endif

    void GoToPlayer()
    {
        SphereCollider hitSphere = LookForPlayer();
        
        if (hitSphere != null)
        {
            if (timeSinceLastRefresh <= lastPOSRefreshRate)
            {
                Debug.Log("ouch");
                timeSinceLastRefresh += Time.deltaTime;
            }
            else
            {
                lastSeenLocation = hitSphere.gameObject.transform.position;
                Debug.DrawLine(transform.position, lastSeenLocation, Color.red, 1f);
                timeSinceLastRefresh = 0;
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
