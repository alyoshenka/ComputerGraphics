using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWobble : MonoBehaviour
{
    public Texture2D heightMap;
    [Range(0, 20)]
    public float heightScale;
    [Range(0, 1)]
    public float speed;

    Mesh mesh;
    MeshCollider meshCol;
    List<Vector2> uvs;
    List<Vector3> verts, origVerts;
    Color[] pixels;

    Vector3 delta;
    Vector2 offset;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        meshCol = GetComponent<MeshCollider>();
        uvs = new List<Vector2>();
        verts = new List<Vector3>();
        origVerts = new List<Vector3>();
        mesh.GetVertices(verts);
        mesh.GetVertices(origVerts);

        mesh.GetUVs(0, uvs);

        offset = Vector2.zero;
        delta = Vector3.zero;
        pixels = heightMap.GetPixels();
    }

    void Update()
    {
        offset.y += heightMap.width * speed * Time.deltaTime;
        CalculateMesh();
    }

    void CalculateMesh()
    {
        for (int i = 0; i < origVerts.Count; i++)
        {
            int idx = (int)(heightMap.width * uvs[i].x)
                + (int)(heightMap.height * uvs[i].y) * heightMap.width;
            idx += (int)offset.y;
            if (idx != 0)
            {
                idx = idx % (pixels.Length - 1);
            }
            delta.y = pixels[idx].grayscale;

            delta.y *= heightScale;
            delta.y -= heightScale / 2;
            verts[i] = origVerts[i] + delta;
        }
        mesh.SetVertices(verts);
        meshCol.sharedMesh = mesh;
    }
}
