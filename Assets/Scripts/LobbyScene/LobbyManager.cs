using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private static LobbyManager instance = null;
    public static LobbyManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    #region 변수 선언

    private readonly string gameVersion = "1.0f";

    [Header("RoomList")]
    public Text idText;
    public Text connectionInfoText;
    [Space(10f)]
    public InputField RoomNameInput;
    [Space(10f)]
    public Button createButton;
    public Button joinRandomButton;
    [Space(10f)]
    public GameObject room;
    public Transform gridTr;

    [Header("RoomMake")]
    public Toggle privateToggle;
    public Toggle publicToggle;
    [Space(10f)]
    public InputField passwordField;
    [Space(10f)]
    public Toggle two;
    public Toggle three;
    public Toggle four;
    [Space(10f)]
    public int maxPlayer;
    public int startMaxPlayer;
    public GameObject createRoomPanel;

    [Header("PrivatePasswordInput")]
    public GameObject privateRoomPwInput;
    public InputField privateRoomPwInputField;
    private char parsing;
    private string[] parsingRoomName;
    private string privateRoomName;

    #endregion 변수 선언

    #region 서버접속을 하기
    // Start is called before the first frame update
    void Start()
    {
        // 서버에 접속
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        // 서버 접속되기 전에는 버튼이 비활성화된다
        createButton.interactable = false;
        joinRandomButton.interactable = false;

        // 서버 접속 메시지
        connectionInfoText.text = "서버에 접속 중입니다...";
        // 서버에 접속


        // 룸 인풋필드에 이름을 기본으로 설정
        RoomNameInput.text = "ROOM_NAME_" + Random.Range(1, 999);
        // 룸 인풋필드에 이름을 기본으로 설정


        // 아이디(닉네임) 정보를 표시
        string id = IDManager.Instance.Nickname();

        idText.text = $"안녕하세요 {id} 님";
        // 아이디(닉네임) 정보를 표시
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();

        // 서버 접속 메시지
        connectionInfoText.text = "온라인 : 서버 접속되었습니다";
    }

    #endregion 서버접속을 하기

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        // 로비에 접속되면 버튼이 활성화된다

        connectionInfoText.text = "온라인 : 게임 접속되었습니다";

        createButton.interactable = true;
        joinRandomButton.interactable = true;
    }

    // 방 만들기
    #region 방 만들기
    public void CreateRoom()
    {
        string roomName;
        string roomPassword = string.Empty;
        string[] displayRoomName;
        string _displayRoomName;

        if (string.IsNullOrEmpty(RoomNameInput.text))
        {
            connectionInfoText.text = "방 이름을 정해주세요";
            return;
        }

        if (passwordField.interactable == true)
        {
            if (string.IsNullOrEmpty(passwordField.text))
            {
                connectionInfoText.text = "비밀번호를 정해주세요";
                return;
            }
            else
            {
                roomPassword = passwordField.text;
            }
        }

        if (roomPassword != String.Empty)
        {
            roomName = $"{RoomNameInput.text}@{roomPassword}";
            char parsing = '@';
            displayRoomName = roomName.Split(parsing);
            _displayRoomName = displayRoomName[0];
        }
        else
        {
            roomName = RoomNameInput.text;
            _displayRoomName = roomName;
        }

        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = (byte)maxPlayer });
        StartPlayer.Instance.maxPlayer = maxPlayer;
        connectionInfoText.text = $"{_displayRoomName} 방을 생성했습니다";

        // 방 만들면 방 만들기 버튼 비활성화된다
        createButton.interactable = false;
    }
    #endregion 방 만들기

    // 랜덤 방 참가
    public void JoinRandomRoom()
    {

        // 접속이 안 되어 있을 경우에 예외처리
        if (PhotonNetwork.IsConnected)
        {
            // 랜덤 방 참가
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // 접속 안 되어 있을때 메시지 그리고 다시 접속 시도
            connectionInfoText.text = "오프라인 : 접속 실패 - 다시 접속 시도합니다...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    // 방 만들기

    // 방 참가 실패했을 경우
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        // 방 참가 실패 메시지
        message = "방이 없습니다";
        connectionInfoText.text = message;

        // 방 참가 실패하면 다시 버튼이 활성화된다
        joinRandomButton.interactable = true;
    }
    // 방 참가 실패했을 경우

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        startMaxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        StartPlayer.Instance.maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;

        // 방 참가하면 버튼이 비활성화된다
        createButton.interactable = false;
        joinRandomButton.interactable = false;

        // 게임 씬으로 이동
        PhotonNetwork.LoadLevel("Game");
    }

    // 룸 리스트를 계속해서 업데이트해준다
    #region 룸 리스트 업데이트
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
        {
            Destroy(obj);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject _room = Instantiate(room, gridTr);
            RoomData roomData = _room.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            parsing = '@';
            parsingRoomName = roomData.roomName.Split(parsing);
            roomData.maxPlayer = roomInfo.MaxPlayers;
            StartPlayer.Instance.maxPlayer = roomInfo.MaxPlayers;
            roomData.playerCount = roomInfo.PlayerCount;
            roomData.UpdateInfo(parsingRoomName[0]);
            roomData.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickRoom(roomData.roomName);
            });
            if (roomData.playerCount == 0)
            {
                Destroy(_room);
            }
        }

    }
    #endregion 룸 리스트 업데이트

    // 룸 버튼을 눌렀을 때
    void OnClickRoom(string roomName)
    {
        Debug.Log(roomName);
        if (roomName.Contains("@"))
        {
            privateRoomPwInput.SetActive(true);
            privateRoomName = roomName;
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName, null);
        }
    }

    public void PrivateOnClickRoom()
    {
        parsing = '@';
        parsingRoomName = privateRoomName.Split(parsing);
        if (privateRoomPwInputField.text == parsingRoomName[1])
        {
            PhotonNetwork.JoinRoom(privateRoomName, null);
        }
        else
        {
            privateRoomPwInput.SetActive(false);
            connectionInfoText.text = "방 비밀번호가 다릅니다";
        }
    }



    // 룸 리스트

    public void RoomMakePanel()
    {
        createRoomPanel.SetActive(true);
    }

    public void RoomMakePanelCancel()
    {
        createRoomPanel.SetActive(false);
    }

    public void ExitButton()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LogIn");
    }

    public void CancelButton()
    {
        privateRoomPwInput.SetActive(false);
    }

    public void ToggleTwo()
    {
        if (two.isOn)
        {
            maxPlayer = 2;
        }
    }

    public void ToggleThree()
    {
        if (three.isOn)
        {
            maxPlayer = 3;
        }
    }

    public void ToggleFour()
    {
        if (four.isOn)
        {
            maxPlayer = 4;
        }
    }

    public void PrivateToggel()
    {
        if (privateToggle.isOn)
        {
            passwordField.interactable = true;
        }
    }

    public void PublicToggle()
    {
        if (publicToggle.isOn)
        {
            passwordField.interactable = false;
        }
    }
}
