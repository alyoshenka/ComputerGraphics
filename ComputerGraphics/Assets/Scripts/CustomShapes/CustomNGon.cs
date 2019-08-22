using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNGon : CustomMesh
{
    [Range(3, 50)]
    public int sides;
    public float radius = 1;

    Vector3[] verts;
    int[] indices;
    Vector2[] UVs;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        verts = new Vector3[sides + 1];
        int i = 0;
        verts[i++] = transform.position;
        for(i = 0; i < verts.Length; i++)
        {
            float angle = Mathf.Deg2Rad * (360f / sides) * i;
            verts[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            verts[i] += transform.position;
        }
        mesh.vertices = verts;

        indices = new int[sides * 3]; // 1 triangle per side
        int triangle = 1;
        for(i = 0; i < indices.Length; i += 3)
        {
            indices[i] = 0;
            indices[i + 1] = triangle;
            if (triangle + 1 >= verts.Length)
            {
                triangle = 0;
            }
            indices[i + 2] = triangle + 1;
            triangle++;
            if (triangle >= verts.Length)
            {
                triangle = 1;
            }
        }
        mesh.triangles = indices;

        Vector3[] norms = new Vector3[verts.Length];
        for(i = 0; i < norms.Length; i++)
        {
            norms[i] = Vector3.forward;
        }
        mesh.normals = norms;

        UVs = new Vector2[verts.Length];
        UVs[0] = new Vector2(0.5f, 0.5f);
        for (i = 0; i < UVs.Length; i++)
        {
            float angle = Mathf.Deg2Rad * (360f / sides) * i;
            UVs[i] = new Vector2(0.5f - Mathf.Cos(angle) / 2f, 0.5f + Mathf.Sin(angle) / 2f);
        }
        mesh.uv = UVs;

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
        customMesh = mesh;
    }

    protected new void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        base.OnDrawGizmos();
    }

    void OnValidate()
    {
        Start();
    }
}
