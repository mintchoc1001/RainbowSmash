using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    public string roomName = "";
    public int playerCount = 0;
    public int maxPlayer = 0;

    private Text roomDataText;

    public void UpdateInfo(string parsingRoomName)
    {
        roomDataText = GetComponentInChildren<Text>();
        roomDataText.text = $"{parsingRoomName} [{playerCount.ToString()} / {maxPlayer}]";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
