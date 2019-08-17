using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CustomMesh : MonoBehaviour
{

    protected Mesh customMesh; // store so it can be destroyed
    protected Vector3[] corners = new Vector3[0];

    void OnDestroy()
    {
        if (null != customMesh)
        {
            Destroy(customMesh);
        }
    }


    protected void OnDrawGizmos()
    {
        foreach (Vector3 v in corners)
        {
            Gizmos.DrawSphere(transform.position + v, 0.1f);
        }
    }
}
