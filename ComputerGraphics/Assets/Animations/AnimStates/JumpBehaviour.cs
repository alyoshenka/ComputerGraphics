using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetTrigger("Impulse");

        Debug.Log("prepare for impulse");
    }
}
