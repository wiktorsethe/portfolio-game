using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashAttackState : StateMachineBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private float speed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        speed = animator.transform.GetComponent<EnemyShip>().moveSpeed;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Sprawdzanie odleg�o�ci mi�dzy wrogiem a ostatni� zapisan� pozycj� gracza
        if (Vector2.Distance(animator.transform.position, animator.transform.GetComponent<EnemyShip>().savedPos) > 5f)
        {
            // Poruszanie wroga w kierunku zapisanej pozycji z podw�jn� pr�dko�ci� ruchu
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, animator.transform.GetComponent<EnemyShip>().savedPos, speed * 2 * Time.deltaTime);
        }
        // Je�li zapisana pozycja gracza jest ju� nieaktualna
        else if (Vector2.Distance(animator.transform.position, animator.transform.GetComponent<EnemyShip>().savedPos) <= 5f)
        {
            // Ustawienie animacji z powrotem do stanu reset
            animator.SetTrigger("Reset");
        }
    }
}
