using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed;

    [Header("Animators")]
    public Animator locomotion;

    [Header("Caps")]
    [SerializeField]
    public Vector3 maxSpeed;

    Rigidbody rb;

    Vector3 input;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        input = input.normalized;
        input *= speed;
        rb.AddForce(input);

        SetAnimator();

        Debug.Log(rb.velocity.z);
    }

    void SetAnimator()
    {
        locomotion.SetFloat("Speed", rb.velocity.z * 0.1f);
        locomotion.SetFloat("Tilt", rb.velocity.x * 0.1f);
    }
}
