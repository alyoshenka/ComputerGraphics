using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionAnimations : MonoBehaviour
{
    [Range(0, 10)]
    public float speed = 0;
    [Range(0, 10)]
    // public float runTrigger = 5;
    public Animator animator;

    void OnValidate()
    {
        animator.SetFloat("Speed", speed);
    }
}
