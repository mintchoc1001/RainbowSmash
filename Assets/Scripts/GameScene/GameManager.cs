using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameState
{
    WAIT,
    PLAY,
    OVER_TIME,
    OVER_WIN,
    OVER_LOSE,
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public List<Player> players = new List<Player>();

    private bool isGameStart;

    //public GameObject playerUI;
    public Transform gridTr;

    Vector3 startPos;
    CapsuleCollider user;
    [Range(-30, -15)] public float startPosXMin;
    [Range(-5, 10)] public float startPosXMax;
    [Range(5, 20)] public float startPosZMin;
    [Range(30, 45)] public float startPosZMax;


    public Text timerUi;
    float timer = 200;
    float countDown = 5;

    bool isPlay = false;

    public GameState state = GameState.WAIT;

    public GameObject ui_Draw;
    public GameObject ui_Win;
    public GameObject ui_Lose;
    public GameObject ui_KeyGuide;
    public GameObject ui_Timer;
    public GameObject ui_Grid;
    public GameObject ui_Goback;
    public GameObject ui_StartInfo;
    public GameObject ui_StartCount;

    public Text startTimer;

    public void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        //if (!isGameStart && PhotonNetwork.CurrentRoom.Players.Count >= 2)
        //    isGameStart = true;

        if (PhotonNetwork.CurrentRoom.MaxPlayers == players.Count)
        {
            state = GameState.PLAY;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //PhotonNetwork.CurrentRoom.RemovedFromList = true;
        }

        switch (state)
        {
            case GameState.WAIT:
                break;

            case GameState.PLAY:
                //PrintUi();
                StartInfo();
                Timer();
                break;

            case GameState.OVER_TIME:
                break;

            case GameState.OVER_WIN:
                break;

            case GameState.OVER_LOSE:
                break;

        }
    }


    public override void OnEnable()
    {
        base.OnEnable();

        CreatePlayer();

        PhotonNetwork.Instantiate("PlayerStatus", Vector3.zero, Quaternion.identity);
    }

    public void SetParent(Transform target, Transform parent)
    {
        target.SetParent(parent);
        target.transform.localScale = Vector3.one;
    }

    void CreatePlayer()
    {
        float rndX = Random.Range(startPosXMin, startPosXMax);
        float rndZ = Random.Range(startPosZMin, startPosZMax);

        startPos = new Vector3(rndX, 0, rndZ);

        GameObject playerName = PhotonNetwork.Instantiate("Player", startPos, Quaternion.identity);
    }


    public void AddPlayer(Player player)
    {
        players.Add(player);
        players = players.OrderBy(p => p.id).ToList();
    }

    void StartInfo()
    {
        ui_StartCount.SetActive(true);
        ui_StartInfo.SetActive(true);

        countDown -= Time.deltaTime;
        startTimer.text = $"{Mathf.Floor(countDown)}";
        if (countDown <= 0)
        {
            ui_StartCount.SetActive(false);
            ui_StartInfo.SetActive(false);
            isGameStart = true;
        }
    }

    public void CheckUser()
    {
        if (players.Count == 1 && players[0].photonView.IsMine)
        {
            state = GameState.OVER_WIN;
            // Win UI 출력
            ui_Win.SetActive(true);
            // 기존 UI = false
            OnResult();
        }
        else if (players.Count == 1 && !players[0].photonView.IsMine)
        {
            state = GameState.OVER_LOSE;
            // Lose UI 출력
            ui_Lose.SetActive(true);
            // 기존 UI = false
            OnResult();
        }
    }

    //void PrintUi()
    //{
    //    ui_Timer.SetActive(true);
    //    ui_KeyGuide.SetActive(true);
    //    ui_Grid.SetActive(true);
    //    ui_Goback.SetActive(true);
    //}

    void OnResult()
    {
        // 기존 UI = false
        ui_Timer.SetActive(false);
        ui_KeyGuide.SetActive(false);
        ui_Grid.SetActive(false);
        ui_Goback.SetActive(false);
    }

    void OnTimeOver()
    {
        state = GameState.OVER_TIME;
        // Draw UI 출력
        ui_Draw.SetActive(true);
        // 기존 UI = false
        ui_Timer.SetActive(false);
        ui_KeyGuide.SetActive(false);
        ui_Grid.SetActive(false);
        ui_Goback.SetActive(false);
    }

    void Timer()
    {
        if (isGameStart == true)
        {
            timer -= Time.deltaTime;
            timerUi.text = $"남은시간 : {Mathf.Floor(timer).ToString()}";

            if (timer <= 0)
                OnTimeOver();
        }
    }

    // 로비로 돌아가기
    public void BackToLobby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
