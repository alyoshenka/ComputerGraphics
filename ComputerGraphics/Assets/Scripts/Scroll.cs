using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [Range(0, 1)]
    public float scrollSpeed;
    public Material mat;

    float scrollElapsed;

    void Start()
    {
        scrollElapsed = 0;
    }

    void Update()
    {
        scrollElapsed += Time.deltaTime * scrollSpeed;
        mat.SetFloat("_ScrollValue", scrollElapsed);
    }
}
