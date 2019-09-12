using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour {

    public float flipOn;
    public float flipOff;
    public bool isOn = false;
    [SerializeField]
    public Vector2 range = new Vector2(.01f, 20f);

    Material emission;
    Color color;

	// Use this for initialization
	void Start () {
        emission = GetComponent<Renderer>().material;
        emission.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (isOn)
        {
            FlipOn();
        }
        else
        {
            FlipOff();
        }

        Debug.Log(emission.GetColor("_EmissionColor"));
    }

    public void FlipOn(float onTime)
    {
        color = emission.GetColor("_EmissionColor");
        if(color.a < range.y)
        {
            emission.SetColor("_EmissionColor", color * (1 + onTime * Time.deltaTime));
        }
     
    }

    public void FlipOn()
    {
        FlipOn(flipOn);
    }

    public void FlipOff(float offTime)
    {
        color = emission.GetColor("_EmissionColor");
        if(color.a > range.x)
        {
            emission.SetColor("_EmissionColor", color * (1 - offTime * Time.deltaTime));
        }
    }

    public void FlipOff()
    {
        FlipOff(flipOff);
    }
}
