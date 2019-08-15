using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModifier : MonoBehaviour {

    public float cycleTime;
    public Color materialColor;
    public List<Texture> textures;

    Material thisMaterial;

    float elapsedCycle;
    int textureIndex;

    // Use this for initialization
    void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        OnValidate();

        elapsedCycle = 0;
        textureIndex = 0;
    }

    void Update()
    {
        elapsedCycle += Time.deltaTime;
        if(elapsedCycle > cycleTime)
        {
            elapsedCycle = 0;
            textureIndex++;
            if(textureIndex >= textures.Count)
            {
                textureIndex = 0;
            }
            thisMaterial.mainTexture = textures[textureIndex];
        }
    }

    void OnValidate()
    {
        if(null != thisMaterial)
        {
            thisMaterial.color = materialColor;
        }
    }
}
