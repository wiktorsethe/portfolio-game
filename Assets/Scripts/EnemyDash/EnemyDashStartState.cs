using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashStartState : StateMachineBehaviour
{
    private GameObject ship;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ship = animator.GetComponent<EnemyShip>().FindClosestObject();
        animator.transform.GetComponent<EnemyShip>().retreatVector = Vector2.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Vector3 vectorToTarget = ship.transform.position - animator.transform.position;
        //animator.transform.Find("EnemyShipImage").transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
        //animator.transform.GetComponent<EnemyDash>().ChangeRotation();
        if (Vector2.Distance(animator.transform.position, ship.transform.position) > 15f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, ship.transform.position, 2f * Time.deltaTime);
        }
        else if (Vector2.Distance(animator.transform.position, ship.transform.position) <= 15f)
        {
            animator.SetTrigger("Attack");
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
