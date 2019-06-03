using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class PlaySoundOnAnimation : StateMachineBehaviour
{
    [SerializeField]
    private Sound sound;
    [SerializeField]
    [Tooltip ("If false it gets the audioSource on the gameObject where the animator is located")]
    private bool isPlayer;
    [SerializeField]
    private bool isEnemy;
    [SerializeField] 
    private float delay = 0f;


    private bool played;
    private PlayerController playC;
    private MultiAudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer) {
            playC = animator.GetComponent<PlayerController>();
        }
        else if (isEnemy)
        {
            audioSource = animator.GetComponent<BasicEnemy>().audioSource;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!played) {
            if (stateInfo.normalizedTime > delay) {
                if (isPlayer) {
                    if (playC != null){
                        AudioManager.Instance.PlaySound(playC.playerAudioSource, sound);
                        played = true;
                    }
                }
                else
                {
                    AudioManager.Instance.PlaySound(audioSource, sound);
                    played = true;
                }
            }  
        } 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        played = false;
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
