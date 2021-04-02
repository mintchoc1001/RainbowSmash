using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class ChatManager : MonoBehaviourPunCallbacks
{
    public List<string> chatList = new List<string>();
    public Button sendBtn;
    public Text[] ChatText; //채팅 내용
    public Text chattingList;
    public InputField ChatInput;
    string chatters;
    private ScrollRect scroll_rect;
    public PhotonView PV;
    private bool isReturn;
    void Start()
    {
        isReturn = false;
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        ChatPanle.SetActive(false);
    }
    public void SendButtonOnClicked()
    {
        if (ChatInput.text.Equals("")) { Debug.Log("Empty"); return; }
        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, ChatInput.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        ChatInput.ActivateInputField(); // 반대는 input.select(); (반대로 토글)
        ChatInput.text = "";
    }
    // Update is called once per frame

    public bool Chatbool; //채팅 활성화유무
    public bool ChatPanlebool; //채팅 패널 활성화 유무
    public GameObject ChatPanle;

    void Update()
    {
       // chatterUpdate();

        if (Input.GetKeyDown(KeyCode.Tab) && ChatPanlebool == false)
        {
            ChatPanle.SetActive(true);
            ChatPanlebool = true;
            ChatInput.interactable = false;
            Chatbool = false;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && ChatPanlebool == true)
        {
            ChatPanle.SetActive(false);
            ChatPanlebool = false;
            Chatbool = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) && ChatPanlebool ==true && Chatbool ==false)
        {
            ChatInput.interactable = true;
            ChatInput.ActivateInputField();
            
        }
        else if(Input.GetKeyDown(KeyCode.Return) && ChatPanlebool == false && Chatbool == true)
        {
            ChatInput.interactable = false;
        }

    }
    void chatterUpdate()
    {
        chatters = "Player List\n";
       /* foreach (Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + "\n";
        }*/
        //chattingList.text = chatters;
    }
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        //chatLog.text += "\n" + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    #region 채팅
    public void Send()
    {
        if (ChatInput.text =="")
        {

        }
        else
        {
        PV.RPC("ChatRPC", RpcTarget.All, IDManager.Instance.Nickname() + " : " + ChatInput.text);
        ChatInput.text = "";

        }
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion
}
