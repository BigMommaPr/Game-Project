using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    private const float Speed = 2.0f;
    
    private Vector2 currentTarget;
    private Rigidbody2D rigidbody;
    
    private static readonly Dictionary<Vector2, Vector2> TargetsPositions = new()
    {
        { new Vector2(7,-3), new Vector2(7, 2) },
        { new Vector2(7, 2), new Vector2(-11, 2) },
        { new Vector2(-11, 2), new Vector2(-11, -3) },
        { new Vector2(-11, -3), new Vector2(7, -3) }
    };
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidbody = animator.GetComponent<Rigidbody2D>();
        currentTarget = new Vector2(7, -3);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(rigidbody.position, currentTarget) < 0.1)
            currentTarget = TargetsPositions[currentTarget];
        var newPosition = Vector2.MoveTowards(rigidbody.position, currentTarget,
            Speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}