using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class GameUI : MonoBehaviourPunCallbacks
{

    Player player;
    BoxCollider meleeArea;

    public GameObject winUi;
    public GameObject loseUi;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player").First(player => player.GetComponent<PhotonView>().IsMine).GetComponent<Player>();
    }
    public override void OnEnable()
    {
        base.OnEnable();

    }

    void Update()
    {
        if (player.count == 7)
        {
            player.isGameover = true;
            meleeArea = GameObject.Find("rarirari").GetComponent<BoxCollider>();
            meleeArea.gameObject.SetActive(false);
            photonView.RPC("WinUi", RpcTarget.Others);
            LoseUi();
        }
    }

    [PunRPC]
    void WinUi()
    {
        winUi.SetActive(true);
    }

    void LoseUi()
    {
        loseUi.SetActive(true);
    }
}
