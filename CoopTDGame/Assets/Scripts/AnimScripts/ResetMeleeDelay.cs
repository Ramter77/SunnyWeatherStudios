using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMeleeDelay : StateMachineBehaviour
{
    [SerializeField]
    private float startDelay = 0.15f;
    [SerializeField]
    private float resetDelay = 0.75f;
    private MeleeAttack meleeAttackScript;
    private bool enabled;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeAttackScript = animator.GetComponent<MeleeAttack>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enabled) {
            if (stateInfo.normalizedTime > startDelay) {
                if (meleeAttackScript != null){
                    meleeAttackScript.ActivateWeaponCollider();
                    enabled = true;
                }
            } 
        }

        if (enabled) {
            if (stateInfo.normalizedTime > resetDelay) {
                if (meleeAttackScript != null){
                    meleeAttackScript.resetMeleeAttackCD();
                }
            }  
        } 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeAttackScript.resetMeleeAttackCD();
        enabled = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //     //Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
