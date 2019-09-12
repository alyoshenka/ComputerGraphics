using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyObject : MonoBehaviour {

    public Button button;
    public Image color1, color2;

    // Use this for initialization
    void Start () {
        button.onClick.AddListener(OnClick);
        OnClick();
	}
	

    void OnClick()
    {
        Color c1 = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1), 1);
        color1.color = c1;
        Color c2 = new Color(1 - c1.r, 1 - c1.g, 1 - c1.b, 1);
        color2.color = c2;
    }
}
