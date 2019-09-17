using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionDiscipline : MonoBehaviour
{
    public ParticleSystem particles;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            particles.Play();
        }

        if (Input.GetMouseButtonDown(1))
        {
            particles.Stop();
        }
    }
}
