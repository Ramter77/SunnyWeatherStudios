using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedCurve : StateMachineBehaviour
{
    [Tooltip ("Add the speed multiplier to the animation")]
    public AnimationCurve SpeedCurve;

    public bool debug;

    private void Awake() {
        //If no speed curve present use default values
        if (SpeedCurve.keys.Length < 1) {
            SpeedCurve = AnimationCurve.Constant(0, 1, 1);
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (debug) {
            Debug.Log("SpeedCurve::::::::::::::::::::::: "+ SpeedCurve.Evaluate(Mathf.Clamp01(stateInfo.normalizedTime)));
        }
        
        animator.SetFloat("AnimationSpeed", SpeedCurve.Evaluate(Mathf.Clamp01(stateInfo.normalizedTime)));
    }
}