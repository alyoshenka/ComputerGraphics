using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttrator : MonoBehaviour
{
    public float radius, strength;
    public List<ParticleSystem> attractedSystems;

    ParticleSystem.Particle[] particles;
    float mag;
    Vector3 dif, pos;
    int max;

    // Start is called before the first frame update
    void Start()
    {
        dif = pos = new Vector3();
        particles = new ParticleSystem.Particle[0];
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ParticleSystem ps in attractedSystems)
        {
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
            max = ps.GetParticles(particles);
            for (int i = 0; i < max; i++)
            {
                // https://answers.unity.com/questions/414829/any-one-know-maths-behind-this-movetowards-functio.html
                pos = particles[i].position;
                dif = transform.position - pos;
                mag = dif.magnitude;
                pos = pos + dif / mag * Mathf.Lerp(0, strength, Mathf.InverseLerp(0, radius, Vector3.Distance(transform.position, pos)));
                particles[i].position += pos;
            }
            ps.SetParticles(particles, max);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
