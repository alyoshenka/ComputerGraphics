using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public float disVal;
    public Material mat;

    float f;

    // Update is called once per frame
    void Update()
    {
        f = Mathf.Sin(Time.realtimeSinceStartup) * disVal;
        f = Mathf.InverseLerp(-1, 1, f);
        mat.SetFloat("_DissolveVal", f);
    }
}
