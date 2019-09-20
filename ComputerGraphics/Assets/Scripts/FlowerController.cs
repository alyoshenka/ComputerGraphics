using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    public PlayerController player;
    public HealthPulse healthEffect;
    public ParticleSystem flower;
    public Texture2D movementNoise;
    public List<Gradient> healthColors;
    public float healthLossSpeed, healthGainSpeed;
    [Range(0, 5)]
    public float speed;
    public float radius;
    [Range(0, 1)]
    public float lerp;
    [Range(0, 0.1f)]
    public float playerToThis;
    public float retarget;

    Vector3 origin;
    Vector3 delta;

    Vector3 target;
    Vector3 targetOrigin;
    float targetRadius;
    float angle;
    float retargetElapsed;

    ParticleSystem.TrailModule flowerTrail;
    int idx;

    void Start()
    {
        origin = transform.position;
        delta = Vector3.zero;

        idx = 0;
        flowerTrail = flower.trails;
        flowerTrail.colorOverTrail = healthColors[idx];

        targetRadius = radius / 1.5f;
        targetOrigin = origin + transform.forward * targetRadius;
        target = targetOrigin;
        retargetElapsed = 0;
    }

    void Update()
    {
        Move();
        ApplyMovementOffset();

        float f = Vector3.Distance(transform.position, origin);
        f = Mathf.Clamp(f, 0, radius);
        idx = (int)Mathf.Lerp(0, healthColors.Count - 1, f / radius);
        flowerTrail.colorOverTrail = healthColors[idx];

        if (idx >= healthColors.Count - 2)
        {
            healthEffect.health -= healthLossSpeed * Time.deltaTime;
        }
        else if(idx <= 1)
        {
            healthEffect.health += healthGainSpeed * Time.deltaTime;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        retargetElapsed += Time.deltaTime;
        if(retargetElapsed > retarget)
        {
            retargetElapsed = 0;
            float f = Random.Range(0, 2 * Mathf.PI);
            target = targetOrigin;
            target.x += Mathf.Cos(f) * targetRadius;
            target.z += Mathf.Sin(f) * targetRadius;
        }
    }

    void ApplyMovementOffset()
    {
        delta.x = player.Velocity.y;
        delta.z = player.Velocity.x;
        delta *= playerToThis;
        transform.position -= delta;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetOrigin, targetRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(target, 0.3f);
    }
}
