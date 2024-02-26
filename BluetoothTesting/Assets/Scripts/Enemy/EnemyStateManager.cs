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
        
    }
}
