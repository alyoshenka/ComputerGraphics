using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColor : MonoBehaviour
{
    Material m;
    void Start()
    {
        m = GetComponent<Renderer>().material;
    }

    public void OneColor()
    {
        m.color = Color.green;
    }

    public void AnotherColor()
    {
        m.color = Color.blue;
    }
}
