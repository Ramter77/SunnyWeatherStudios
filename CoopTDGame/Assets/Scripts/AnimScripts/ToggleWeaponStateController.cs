using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWeaponStateController : StateMachineBehaviour{
    


    public override void OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {

       if(animatorStateInfo.normalizedTime > .5f ){
           //if(animator.GetComponent<CharacterCombat>() != null){
             //  animator.GetComponent<CharacterCombat>().ToggleWeapon();
           //}
       }


    }

}
