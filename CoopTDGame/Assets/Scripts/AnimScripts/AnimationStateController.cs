using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : StateMachineBehaviour{
    
    public AnimationCurve _SpeedCurve;



    private void Awake() {
        
        if(_SpeedCurve.keys.Length < 1) {
            _SpeedCurve = AnimationCurve.Constant(0, 1, 1);
        }

        
    }

    AnimationClip _Clip;

    float _NormTime;

    public override void OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {

        
        
        animator.SetFloat("AttackAnimSpeed", _SpeedCurve.Evaluate(Mathf.Clamp01(animatorStateInfo.normalizedTime)));

        

        //Debug.Log(animator.speed);
        //if(animatorStateInfo.normalizedTime > 1) Debug.Log("Above 100%" + animatorStateInfo.normalizedTime);
        //Debug.Log("layer: " + layerIndex + "\n State: " + _Clip.name);
        //Debug.Log("Total: " + _Clip.length + "\nCurrent: "  + "Percent: " + animatorStateInfo.normalizedTime + "\nPercent: " + _Clip.length / animatorStateInfo.length);
        //animator.speed = _SpeedCurve.Evaluate(_Clip.length / animatorStateInfo.length);
    }

}
