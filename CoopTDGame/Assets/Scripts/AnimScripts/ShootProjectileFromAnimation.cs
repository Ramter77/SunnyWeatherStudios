using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class ShootProjectileFromAnimation : StateMachineBehaviour
{
    [SerializeField]
    private float soundDelay = 0.1f;
    [SerializeField]
    private float shootDelay = 0.5f;

    
    private bool playedSound, shot;
    private PlayerController playC;
    private Element element;
    private RangedAttack rangedAttackScript;
    private MultiAudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playC == null) {
            playC = animator.GetComponent<PlayerController>();
            element = playC.Element;
        }
        if (rangedAttackScript == null) {
            rangedAttackScript = animator.GetComponent<RangedAttack>();
        }
        audioSource = animator.GetComponent<MultiAudioSource>();

        shot = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!shot) {
            if (stateInfo.normalizedTime > shootDelay) {
                if (!playC.isMeleeAttacking) {
                    if (!playedSound) {
                        if (element == Element.Fire) {
                            AudioManager.Instance.PlaySound(audioSource, Sound.towerFire);
                        }
                        else if (element == Element.Ice) {
                            AudioManager.Instance.PlaySound(audioSource, Sound.towerIce);
                        }
                        playedSound = true;
                    }

                    rangedAttackScript.ShootActiveProjectile();
                    shot = true;
                }
            } 
        }   
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playedSound = false;
        shot = false;
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
