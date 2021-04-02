using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    RectTransform gameUi;

    public float speed;

    void Start()
    {
        gameUi = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(gameUi.localPosition.y<500)
        gameUi.localPosition+= Vector3.up * Time.deltaTime * speed;

    }
}
