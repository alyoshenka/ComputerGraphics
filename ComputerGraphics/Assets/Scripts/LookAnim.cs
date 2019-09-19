using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LookAnim : MonoBehaviour
{
    public Transform lookTransform;

    Animator animator;
    Camera cam;
    Vector3 look;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        look = new Vector3();
    }

    void Update()
    {
        look = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(null == lookTransform ? look : lookTransform.position);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(null == lookTransform ? look : lookTransform.position, 0.1f);
    }
}
