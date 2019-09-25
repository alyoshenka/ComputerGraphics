using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    public Transform mainCam;
    public List<Transform> path;
    public Transform target;
    public float speed;
    public float epsilon;

    int pathIdx;

    // Start is called before the first frame update
    void Start()
    {
        pathIdx = 0;
        mainCam.transform.position = path[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = Vector3.MoveTowards
            (mainCam.transform.position, path[pathIdx].position, speed * Time.deltaTime);
        if(Vector3.Distance(mainCam.transform.position, path[pathIdx].position) < epsilon)
        {
            pathIdx++;
            if(pathIdx >= path.Count)
            {
                mainCam.transform.rotation = path[path.Count - 1].rotation;
                enabled = false;
            }
        }
        mainCam.transform.LookAt(target);
    }

    void OnDrawGizmos()
    {
        for(int i = 0; i < path.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(path[i].position, 2);
            if(i < path.Count - 1)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
            }
        }
    }
}
