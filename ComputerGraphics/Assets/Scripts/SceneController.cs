using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("How fast the character is running")]
    [Range(0, 1)]
    public float runSpeed;
    [Tooltip("Where the character is leaning")]
    [Range(-0.5f, 0.5f)]
    public float leanVal;

    [Header("Component variables")]
    [Tooltip("Ratio of player forward speed to ground forward speed")]
    [Range(0, 1)]
    public float playerToGroundF;
    [Tooltip("Ratio of player sideways speed to ground sideways speed")]
    [Range(0, 1)]
    public float playerToGroundS;

    [Header("Components")]
    [Tooltip("Player animator")]
    public Animator animator;
    [Tooltip("Scroll controller")]
    public Scroll groundScroll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        animator.SetFloat("Speed", runSpeed);
        animator.SetFloat("Tilt", leanVal);
        groundScroll.scrollForward = runSpeed * playerToGroundF;
        groundScroll.scrollSideways = leanVal * playerToGroundS;
    }
}
