using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeCube : CustomMesh {

    Vector3[] norms;
    Vector2[] UVs;

    // Use this for initialization
    void Start () {
        Mesh mesh = new Mesh();

        corners = new Vector3[8];
        corners[0] = new Vector3(0, 0, 0);
        corners[1] = new Vector3(0, 0, 1);
        corners[2] = new Vector3(1, 0, 1);
        corners[3] = new Vector3(1, 0, 0);
        corners[4] = new Vector3(0, 1, 0);
        corners[5] = new Vector3(0, 1, 1);
        corners[6] = new Vector3(1, 1, 1);
        corners[7] = new Vector3(1, 1, 0);

        Vector3[] verts = new Vector3[24];
        verts[0] = corners[4]; // t
        verts[1] = corners[5];
        verts[2] = corners[6];
        verts[3] = corners[7];

        verts[4] = corners[0]; // bo
        verts[5] = corners[1];
        verts[6] = corners[2]; 
        verts[7] = corners[3];

        verts[8] = corners[0]; // l
        verts[9] = corners[1];
        verts[10] = corners[5];
        verts[11] = corners[4];

        verts[12] = corners[3]; // r
        verts[13] = corners[7];
        verts[14] = corners[6];
        verts[15] = corners[2];

        verts[16] = corners[0]; // f
        verts[17] = corners[4];
        verts[18] = corners[7];
        verts[19] = corners[3];

        verts[20] = corners[1]; // ba
        verts[21] = corners[5];
        verts[22] = corners[6];
        verts[23] = corners[2];
        mesh.vertices = verts;

        int[] indices = new int[36];
        indices[0] = 0; // t
        indices[1] = 1;
        indices[2] = 2;

        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;

        indices[6] = 4; // bo
        indices[7] = 6;
        indices[8] = 5;

        indices[9] = 4;
        indices[10] = 7;
        indices[11] = 6;

        indices[12] = 9; // l
        indices[13] = 10;
        indices[14] = 8;

        indices[15] = 8;
        indices[16] = 10;
        indices[17] = 11;

        indices[18] = 12; // r
        indices[19] = 13;
        indices[20] = 14;

        indices[21] = 12;
        indices[22] = 14;
        indices[23] = 15;

        indices[24] = 16; // f
        indices[25] = 17;
        indices[26] = 18;

        indices[27] = 16;
        indices[28] = 18;
        indices[29] = 19;

        indices[30] = 20; // ba
        indices[31] = 22;
        indices[32] = 21;

        indices[33] = 20;
        indices[34] = 23;
        indices[35] = 22;
        mesh.triangles = indices;

        norms = new Vector3[24];
        SetNormals(0, 3, Vector3.up);
        SetNormals(4, 7, Vector3.down);
        SetNormals(8, 11, Vector3.left);
        SetNormals(12, 15, Vector3.right);
        SetNormals(16, 19, Vector3.back);
        SetNormals(20, 23, Vector3.forward);
        mesh.normals = norms;

        UVs = new Vector2[24];
        UVs[0] = UVs[5] = UVs[9]  = UVs[12] = UVs[16] = UVs[23] = new Vector2(0, 0);
        UVs[2] = UVs[7] = UVs[11] = UVs[14] = UVs[18] = UVs[21] = new Vector2(1, 1);
        UVs[1] = UVs[4] = UVs[10] = UVs[13] = UVs[17] = UVs[22] = new Vector2(0, 1);
        UVs[3] = UVs[6] = UVs[8]  = UVs[15] = UVs[19] = UVs[20] = new Vector2(1, 0);
        mesh.uv = UVs;

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
        customMesh = mesh;
	}

    void SetNormals(int start, int end, Vector3 normal)
    {
        for(int i = start; i <= end; i++)
        {
            norms[i] = normal;
        }
    }

    protected new void OnDrawGizmos()
    {
        Gizmos.color = new Color(153, 0, 204, 1);
        foreach (Vector3 v in corners)
        {
            Gizmos.DrawSphere(transform.position + v, 0.1f);
        }

        //for(int i = 0; i < verts.Length; i++){
        //    Gizmos.DrawLine(transform.position + verts[i], transform.position + verts[i] + norms[i]);
        //}
    }
}
