using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachAnim : MonoBehaviour
{
    public Transform lookTransform;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, lookTransform.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, lookTransform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, lookTransform.rotation);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lookTransform.rotation);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(lookTransform.position, 0.1f);
    }
}
