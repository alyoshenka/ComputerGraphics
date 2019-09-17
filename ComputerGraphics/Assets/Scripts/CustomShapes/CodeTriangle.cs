using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTriangle : CustomMesh {

    void Start () {
        Mesh mesh = new Mesh();

        // locations
        Vector3[] verts = new Vector3[3];
        verts[0] = new Vector3(0, 0, 0);
        verts[1] = new Vector3(0, 1, 0);
        verts[2] = new Vector3(1, 0, 0);
        mesh.vertices = verts;

        // which locations to use
        // needs to be multiple of 3 (triangle) in CW order
        int[] indices = new int[3];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        mesh.triangles = indices;

        // how light bounces off the surface
        // interpolated across surface
        Vector3[] norms = new Vector3[3];
        norms[0] = norms[1] = norms[2] = -Vector3.forward;
        mesh.normals = norms;

        // how textures are mapped to surface
        Vector2[] UVs = new Vector2[3];
        UVs[0] = new Vector2(0, 0);
        UVs[1] = new Vector2(0, 1);
        UVs[2] = new Vector2(1, 0);
        mesh.uv = UVs;

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
        customMesh = mesh;
	}


    protected new void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        base.OnDrawGizmos();
    }
}
