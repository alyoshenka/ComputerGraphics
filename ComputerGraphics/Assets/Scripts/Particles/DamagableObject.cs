using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObject : MonoBehaviour
{
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void TakeDamage()
    {
        mat.color = Color.red;
    }
}
