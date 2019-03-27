using UnityEngine;

public class CharacterStateBehaviour : StateMachineBehaviour{
    [HideInInspector]
    public AnimatorStateInfo currentState;
    private AvatarMask originalAvatarMask;
    public AvatarMask newAvatarMask;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
       currentState = stateInfo;
       
        //AnimationClip clip  = animator.GetLayer(0).stateMachine.state.GetMotion() as AnimationClip

        //animator.SetBool("Chain", false);
        //animator.SetInteger("ComboInput", 0);


        //Debug.Log("State Enter");


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        //Debug.Log("State Exit");

        
       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
       
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(stateInfo.normalizedTime);
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("On Attack IK ");
    }
}