using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePentagon : CustomMesh
{
    void Start()
    {
        Mesh mesh = new Mesh();

        // locations
        verts = new Vector3[7];
        verts[0] = new Vector3(1, 0, 0);
        verts[1] = new Vector3(0, 3, 0);
        verts[2] = new Vector3(2, 4, 0);
        verts[3] = new Vector3(4, 3, 0);
        verts[4] = new Vector3(3, 0, 0);
        verts[5] = new Vector3(1, 3, 0);
        verts[6] = new Vector3(3, 3, 0);
        mesh.vertices = verts;

        // which locations to use
        // needs to be multiple of 3 (triangle) in CW order
        int[] indices = new int[15]; // 3*5=15
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 5;

        indices[3] = 1;
        indices[4] = 2;
        indices[5] = 3;

        indices[6] = 4;
        indices[7] = 6;
        indices[8] = 3;

        indices[9] = 0;
        indices[10] = 5;
        indices[11] = 6;

        indices[12] = 0;
        indices[13] = 6;
        indices[14] = 4;
        mesh.triangles = indices;

        // how light bounces off the surface
        // interpolated across surface
        Vector3[] norms = new Vector3[verts.Length];
        for(int i = 0; i < norms.Length; i++)
        {
            norms[i] = -Vector3.forward;
        }
        mesh.normals = norms;

        // how textures are mapped to surface
        Vector2[] UVs = new Vector2[verts.Length];
        UVs[0] = new Vector2(0.25f, 0);
        UVs[1] = new Vector2(0, 0.75f);
        UVs[2] = new Vector2(0.5f, 1);
        UVs[3] = new Vector2(1, 0.75f);
        UVs[4] = new Vector3(0.75f, 0);
        UVs[5] = new Vector2(0.25f, 0.75f);
        UVs[6] = new Vector2(0.75f, 0.75f);
        mesh.uv = UVs;

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
        customMesh = mesh;
    }


    protected new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        base.OnDrawGizmos();
    }
}
