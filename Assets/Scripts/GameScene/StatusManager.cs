using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
public class StatusManager : MonoBehaviourPunCallbacks
{
    public Player player;
    public GameObject[] colors;

    void Start()
    {
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        
        UiControl();
    }

    public void UiControl()
    {
        if (player.count >= 1 && player.count <= 7)
        {
            colors[player.count - 1].SetActive(false);
        }
    }
}
