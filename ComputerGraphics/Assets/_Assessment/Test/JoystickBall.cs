using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ToDo
// fix pointer logic
// organize files
// organize repo

[RequireComponent(typeof(RectTransform))]
public class JoystickBall : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public Image ball;
    public Transform origin;

    public Color hoverColor;
    public Color selectionColor;
    public Color baseColor;

    public float returnSpeed;
    public float radius;

    float lerpSpeed;
    bool isHovering;
    bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        ball.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!isSelected)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, 
                origin.position, returnSpeed * Time.deltaTime); // DONT MOVE
        }
        else */
        if (isSelected && Vector3.Distance(origin.position, ball.transform.position) < radius)
        {
            ball.transform.position = Input.mousePosition;
        }
    }

    public Vector2 Value()
    {
        return new Vector2(ball.transform.position.x - origin.position.x, ball.transform.position.y - origin.position.y).normalized;
    }

    public void ResetPosition()
    {
        ball.transform.position = origin.position;
    }

    #region Pointer Events

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");

        isHovering = true;
        ball.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");

        isHovering = false;
        ball.color = baseColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");

        if (isHovering)
        {
            isSelected = true;
            ball.color = selectionColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");

        isSelected = false;
        ball.color = isHovering ? hoverColor : baseColor;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin.position, radius);
    }
}
