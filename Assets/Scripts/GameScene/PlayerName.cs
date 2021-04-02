using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class PlayerName : MonoBehaviourPunCallbacks
{
    Player player;

    public StatusManager playerStatus;

    private void Start()
    {
    }

    private void OnEnable()
    {

        player = GameManager.Instance.players.First(p => p.playerStatusUI == null);
        player.playerStatusUI = playerStatus;
        playerStatus.player = player;
        GameManager.Instance.SetParent(playerStatus.transform, GameManager.Instance.gridTr);
    }

    private void Update()
    {
        GetComponent<Text>().text = player.playerName;
    }

}
