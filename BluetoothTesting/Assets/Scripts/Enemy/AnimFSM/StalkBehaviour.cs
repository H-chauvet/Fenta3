using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkBehaviour : StateMachineBehaviour
{
    private GameObject thisObject;
    private EnemyStateManager ESM;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thisObject = animator.gameObject;
        ESM = thisObject.GetComponent<EnemyStateManager>();
        ESM.currentState = EnemyStateManager.EnemyState.STALKING;
        if(!ESM.enemySightSource.isPlaying)ESM.enemySightSource.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ESM.Stalk();
        if(!ESM.enemyChaseSource.isPlaying)ESM.enemyChaseSource.Play();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ESM.enemyChaseSource.Stop();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
