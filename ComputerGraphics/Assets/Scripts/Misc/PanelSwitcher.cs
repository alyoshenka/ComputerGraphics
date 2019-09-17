using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour {

    public GameObject baseObject;
    public Transform objectParent;
    [SerializeField]
    public Vector3 objectOffset;
    public int yOffset;
    public Button addButton;
    public RectTransform content;

    Canvas canvas;
    Vector3 currentOffset;
    List<RectTransform> scrollList;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        addButton.onClick.AddListener(Add);
        currentOffset = Vector3.zero;
        currentOffset = objectOffset;
        scrollList = new List<RectTransform>();
	}
	
    void Add()
    {
        GameObject g = Instantiate(baseObject, objectParent);
        g.transform.position = objectParent.position + currentOffset;
        currentOffset.y -= yOffset;
        Vector3 pos = content.position;
        pos.y -= yOffset / 2;
        content.position = pos;
        pos = content.sizeDelta;
        pos.y += yOffset;
        content.sizeDelta = pos;
        scrollList.Add(g.GetComponent<RectTransform>());
        for(int i = 0; i < scrollList.Count; i++)
        {
            pos = scrollList[i].position;
            pos.y = content.sizeDelta.y - ((yOffset * i) / 2);
            scrollList[i].position = pos;
        }
    }
}
