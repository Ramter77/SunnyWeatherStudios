using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileFromAnimation : StateMachineBehaviour
{
    public float shootDelay = 0.5f;
    private bool shot;
    private PlayerController playC;
    private RangedAttack rangedAttackScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playC == null) {
            playC = animator.GetComponent<PlayerController>();
        }
        if (rangedAttackScript == null) {
            rangedAttackScript = animator.GetComponent<RangedAttack>();
        }

        shot = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > shootDelay) {
            ThrowProjectile();
        }    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

    void ThrowProjectile() {
        if (!shot) {
            if (!playC.isJumping) {
                shot = true;
                rangedAttackScript.ShootActiveProjectile();
            }
        }
    }
}
