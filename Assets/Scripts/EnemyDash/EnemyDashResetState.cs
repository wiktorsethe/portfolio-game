using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashResetState : StateMachineBehaviour
{
    private float timer = 0f;
    private CameraShake camShake;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        //animator.updateMode = AnimatorUpdateMode.Normal;
        camShake = GameObject.FindObjectOfType(typeof(CameraShake)) as CameraShake;
        camShake.ShakeCamera(0.3f, 1f, 6);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            //animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            animator.SetTrigger("Retreat");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
