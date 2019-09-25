using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDebug : MonoBehaviour
{
    public Material mat;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mat.GetFloat("a"));
    }
}
