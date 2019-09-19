using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWobble : MonoBehaviour
{
    public Texture2D heightMap;
    [Range(0, 20)]
    public float heightScale;
    [Range(1, 50)]
    public float period;
    public float speed;

    Mesh mesh;
    MeshCollider meshCol;
    List<Vector2> uvs;
    List<Vector3> verts, origVerts;
    Color[] pixels;

    Vector3 delta;
    Vector2 offset;

    int total;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
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

        total = 0;
    }

    // Update is called once per frame
    void Update()
    {
        total++;
        offset.y += heightMap.width * speed * Time.deltaTime;
        for (int i = 0; i < origVerts.Count; i++)
        {
            // int x = (int)Mathf.Floor(heightMap.width * (uvs[i].x / heightMap.width));

            int idx = (int)(heightMap.width * uvs[i].x) + (int)(heightMap.height * uvs[i].y) * heightMap.width;
            idx += (int)offset.y;
            if (idx != 0) { idx = (pixels.Length - 1) % idx; }
            delta.y = pixels[idx].grayscale;
            // delta.y = Mathf.Lerp(-1, 1, delta.y);
            // delta.y = Mathf.Sin((uvs[i].y + offset.y) * period);

            delta.y *= heightScale;
            delta.y -= heightScale / 2;
            verts[i] = origVerts[i] + delta;
        }
        mesh.SetVertices(verts);
        meshCol.sharedMesh = mesh;
    }

    void OnDestroy() // needed?
    {
        if (null != mesh)
        {
            Destroy(mesh);
        }
    }
}
