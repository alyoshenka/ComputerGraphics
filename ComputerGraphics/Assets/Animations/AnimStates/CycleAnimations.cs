using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleAnimations : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("First");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("Second");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("Third");
        }
    }
}
