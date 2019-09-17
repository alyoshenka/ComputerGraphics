using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObject : MonoBehaviour
{
    public float rechargeTime = 2;

    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void TakeDamage()
    {
        mat.color = Color.red;
        StartCoroutine("Recharge");
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        mat.color = Color.white;
    }
}
