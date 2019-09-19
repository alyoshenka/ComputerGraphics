using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo
// player to flower motion

public class FlowerController : MonoBehaviour
{
    public PlayerController player;
    public Texture2D movementNoise;
    public float speed;
    public float radius;
    [Range(0, 1)]
    public float lerp;
    [Range(0, 0.1f)]
    public float playerToThis;

    Vector3 origin;
    Vector3 delta;

    void Start()
    {
        origin = transform.position;
        delta = Vector3.zero;
    }

    void Update()
    {
        SampleTexture();
        ApplyMovementOffset();

        transform.position = Vector3.Lerp(transform.position, origin + delta, lerp);
    }

    void SampleTexture()
    {
        /*
        delta.x = movementNoise.GetPixel((int)(Time.realtimeSinceStartup * speed), 5).grayscale;
        delta.z = movementNoise.GetPixel(10, (int)(Time.realtimeSinceStartup * speed)).grayscale;
        Debug.Log(delta.x);
        delta.x *= Mathf.Sin(Time.realtimeSinceStartup);
        delta *= radius;
        */
        delta.z += speed * Time.deltaTime;
    }

    void ApplyMovementOffset()
    {
        delta.x -= player.Velocity.y * playerToThis;
        delta.z -= player.Velocity.x * playerToThis;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
