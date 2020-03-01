using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_walk : StateMachineBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackRange = 1f;

    Transform player;
    Rigidbody2D rigidbody;
    Monster monster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        monster = animator.GetComponent<Monster>();
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Can_Attack())
        {
            animator.SetTrigger("Attack");
            return;
        }

        Vector2 target = new Vector2(player.position.x, rigidbody.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, target, moveSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);
        monster.LookAt_Player(player);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    bool Can_Attack()
    {
        return Mathf.Abs(rigidbody.position.x - player.position.x) < attackRange;
    }
}
