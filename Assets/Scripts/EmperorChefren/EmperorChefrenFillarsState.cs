using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmperorChefrenFillarsState : StateMachineBehaviour
{
    private Vector2 startingPos;
    private float timer = 0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        startingPos = animator.GetComponent<EmperorChefren>().startingPos;
        animator.GetComponent<EmperorChefren>().Pillars();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EmperorChefren>().ChangeRotation();
        timer += Time.deltaTime;
        if (Vector2.Distance(animator.transform.position, startingPos) <= 0.01f)
        {
            //animator.SetTrigger("");
        }
        else if (Vector2.Distance(animator.transform.position, startingPos) > 0.01f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, startingPos, 20f * Time.deltaTime);
        }

        if(timer >= 3f)
        {
            animator.SetTrigger("Attack1");
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
