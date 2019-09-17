using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FlameThrower : MonoBehaviour
{
    ParticleSystem particles;
    List<ParticleCollisionEvent> collisions;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        DamagableObject damObj = other.GetComponent<DamagableObject>();
        damObj?.TakeDamage();
    }
}
