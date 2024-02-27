using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public enum EnemyState{
        PATROL,
        STALKING,
        CHASING,
        TRACKING
    }
    
    

    public EnemyState currentState;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.STALKING:
                Stalk();
                break;
            case EnemyState.CHASING:
                Chase();
                break;
            case EnemyState.TRACKING:
                Track();
                break;
        }
    }

    public void Patrol()
    {
        
    }

    public void Stalk()
    {
        
    }

    public void Chase()
    {
        
    }

    public void Track()
    {
        
    }
}
