using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Vector2 maxSpeed;
    [SerializeField]
    public Vector2 acceleration;
    [SerializeField]
    public Vector2 deceleration;
    [Range(0, 0.2f)]
    public float epsilon;
    public Animator animator;

    public Vector2 Velocity { get { return velocity; } private set { } }

    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        ApplyInput();
    }

    void TakeInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity.x += acceleration.x * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.x -= acceleration.x * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity.y -= acceleration.y * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.y += acceleration.y * Time.deltaTime;
        }
        else
        {
            if(Mathf.Abs(velocity.y) < epsilon)
            {
                velocity.y = 0;
            }
            else if(velocity.y > 0)
            {
                velocity.y -= deceleration.y * Time.deltaTime;
            }
            else
            {
                velocity.y += deceleration.y * Time.deltaTime;
            }
        }

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed.x, maxSpeed.x);
        velocity.y = Mathf.Clamp(velocity.y, -maxSpeed.y, maxSpeed.y);
    }

    void ApplyInput()
    {
        animator.SetFloat("Speed", velocity.x / maxSpeed.x);
        animator.SetFloat("Tilt", velocity.y / maxSpeed.y);
    }
}
