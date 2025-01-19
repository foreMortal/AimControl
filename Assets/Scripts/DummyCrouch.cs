using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCrouch : StateMachineBehaviour
{
    private Transform look;
     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        look = animator.GetComponentInChildren<LookAtPlayer>().transform;
        look.SetLocalPositionAndRotation(new Vector3(-0.035f, 1.1f, 0.731f), Quaternion.identity);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        look.SetLocalPositionAndRotation(new Vector3(-0.035f, 1.614f, 0.731f), Quaternion.identity);
    }
}
