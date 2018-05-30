using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameTimer : NetworkBehaviour {

    [SyncVar]
    public float gameTime;

    [SyncVar]
    public float timer = -1;

    [SyncVar]
    public float minPlayers;

    [SyncVar]
    public bool masterTimer = false;

    GameTimer serverTimer;

    [SerializeField]
    Text timerText;

    [SerializeField]
    GameObject gameOver;


    bool isGameOver = false;

    private NetworkManager networkManager;

    private void Start()
    {

        isGameOver = false;
        networkManager = NetworkManager.singleton;

        if(isServer)
        {
            serverTimer = this;
            masterTimer = true;
        }
        else if(isLocalPlayer)
        {
            GameTimer[] timers = FindObjectsOfType<GameTimer>();
            for(int i = 0; i < timers.Length; i++)
            {
                if(timers[i].masterTimer)
                {
                    serverTimer = timers[i];
                }
            }
        }
        timer = gameTime;
    }

    private void Update()
    {
        if(masterTimer)
        {
            if (NetworkServer.connections.Count < minPlayers)
            {
                timer = -10;
            }
            else if (timer <= gameTime && timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0 && timer >-2)//Time is up
            {
                timer = -5;
                isGameOver = true;
            }
            else if(timer == -5)//Game Over
            {
                GameOver();
            }
            
            else
            {
                timer = gameTime;
            }
        }

        if(isLocalPlayer)
        {
            if(serverTimer)
            {
                gameTime = serverTimer.gameTime;
                timer = serverTimer.timer;
                minPlayers = serverTimer.minPlayers;
            }
            else
            {
                GameTimer[] timers = FindObjectsOfType<GameTimer>();
                for(int i = 0; i<timers.Length; i++)
                {
                    if(timers[i].masterTimer)
                    {
                        serverTimer = timers[i];
                    }
                }
            }
        }

        string minutes = ((int)timer / 60).ToString();
        string seconds = (timer % 60).ToString("f0");
        timerText.text = minutes + ":" + seconds;

        if(isGameOver)
        {
            timerText.text = "Game Over";
            timerText.fontSize = 14;
        }
        //timerText.text = timer.ToString();
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        GameManager.instance.scoreboard.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        timerText.gameObject.SetActive(false);

        GameManager.instance.scoreboard.SetActive(true);
        GameManager.instance.SetSceneCameraActive(true);

        Player[] players = GameManager.GetAllPlayers();
        foreach (Player player in players)
        {
            player.gameObject.SetActive(false);
        }
    }
    
    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
