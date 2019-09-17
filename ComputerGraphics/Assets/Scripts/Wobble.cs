using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float wobbleSpeed;
    [SerializeField]
    public Vector3 wobbleStrength;
    public Material mat;

    // Update is called once per frame
    void Update()
    {
        mat.SetVector("_Wobble", Mathf.Sin(wobbleSpeed * Time.realtimeSinceStartup) * wobbleStrength);
    }
}
