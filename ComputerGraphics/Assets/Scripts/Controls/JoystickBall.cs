using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// a UI joystick element
/// </summary>
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

    void Start()
    {
        ball.color = baseColor;
    }

    void Update()
    {
        if (Vector3.Distance(origin.position, ball.transform.position) < radius)
        {
            if (isSelected && isHovering)
            {
                ball.transform.position = Input.mousePosition;
            }
        }
        else
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, origin.position, returnSpeed * Time.deltaTime);
        }
    }

    public Vector2 Value()
    {
        return new Vector2((ball.transform.position.x - origin.position.x) / -radius, 
            (ball.transform.position.y - origin.position.y) / -radius);
    }

    public void ResetPosition()
    {
        ball.transform.position = origin.position;
    }

    #region Pointer Events

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ball.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        ball.color = baseColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHovering)
        {
            isSelected = true;
            ball.color = selectionColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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
