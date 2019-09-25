using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromA : MonoBehaviour
{
    [Range(-0.1f, 0.1f)]
    public float speedX;
    [Range(-0.1f, 0.1f)]
    public float speedY;
    [Range(-0.1f, 0.1f)]
    public float speedZ;
    [Range(0, 0.01f)]
    public float max;
    [Range(-0.01f, 0)]
    public float min;
    public Material mat;

    Vector3 val;

    // Start is called before the first frame update
    void Start()
    {
        val = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        val.x += speedX * Time.deltaTime;
        val.y += speedY * Time.deltaTime;
        val.z += speedZ * Time.deltaTime;
        if (val.magnitude > max || val.magnitude < min)
        {
            speedX *= -1;
            speedY *= -1;
            speedZ *= -1;
        }
        mat.SetVector("_Offset", val);
    }
}
