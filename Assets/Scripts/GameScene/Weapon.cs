using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BoxCollider meleeArea;
    public int damage;

    public GameObject p;

    public void Use()
    {
        StopCoroutine("Swing");
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.1f);
    }

}
