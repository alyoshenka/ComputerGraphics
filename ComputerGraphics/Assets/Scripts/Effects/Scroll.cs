using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [Range(0, 1)]
    public float scrollForward;
    [Range(-0.5f, 0.5f)]
    public float scrollSideways;
    public Material mat;

    float scrollForwardElapsed;
    float scrollSidewaysElapsed;

    void Start()
    {
        scrollForwardElapsed = scrollSidewaysElapsed= 0;
    }

    void Update()
    {
        scrollForwardElapsed += Time.deltaTime * scrollForward;
        scrollSidewaysElapsed += Time.deltaTime * scrollSideways;
        mat.SetFloat("_ScrollForward", scrollForwardElapsed);
        mat.SetFloat("_ScrollSideways", scrollSidewaysElapsed);
    }
}
