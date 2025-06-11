using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Play,
    Pause,
    Stop
}

public class GameManager : Singleton<GameManager>
{
    //플레이어가 진행중인 맵
    [SerializeField] Map playMap;
    //브금
    public AudioSource bgm;

    [Range(0, 1)] public float soundSize = 1f;
    [Range(0, 1)] public float bgmSize = 0.15f;

    //플레이어
    public Player player;
    public Player playerPrefab;
    //게임 속도
    public float gameSpeed = 1;
    //게임 상태
    public GameState state;

    //등급에 맞는 무기만 넣어두고 상황에 맞게 꺼내쓰기 위함
    public List<Item> commonItems = new List<Item>();
    public List<Item> normalItems = new List<Item>();
    public List<Item> rareItems = new List<Item>();
    public List<Item> epicItems = new List<Item>();
    public List<Item> legendItems = new List<Item>();

    public UIManager ui;
    [SerializeField] UIManager uiprefab;

    //슬로우모션 지속시간
    public float slowTime = 4f;
    bool isSlowMotion = false;

    private void Awake()
    {
        state = GameState.Play;
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (!isSlowMotion)
        {
            Time.timeScale += 1f / slowTime * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            MapManager.instance.EntryLastMap();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.mobStat.hp = player.mobStat.max_hp = 500;
            player.mobStat.damage = 500;
        }
    }

    //플레이어가 포탈을 통해 맵 이동시 playMap을 다음 맵으로 바꿔줌
    public static Map Map
    {
        get
        {
            if (Instance == null) return null;
            return Instance.playMap;
        }
        set
        {
            if (Instance == null) return;
            Instance.playMap = value;
        }
    }

    static public Player GetPlayer()
    {
        if (Instance == null) return null;
        return Instance.player;
    }

    static public float Speed
    {
        get
        {
            if (Instance == null) return 1;
            return Instance.gameSpeed;
        }
        set { Instance.gameSpeed = value; }
    }

    static public float deltaTime
    {
        get
        {
            if (Instance == null) return Time.deltaTime;
            return Instance.gameSpeed * Time.deltaTime;
        }
    }

    //playmap으로 카메라를 세팅해줌
    public void MapCamera()
    {
        CameraManager.Instance.SetPosSize(player.transform.position, Map.camera_MaxSize, Map.Camera_MinSize);
    }

    static public void SetUI(bool active)
    {
        if (active == true && Instance.ui == null)
        {
            Instance.ui = Instantiate(Instance.uiprefab);
        }
        else
        {
            UIManager.ReSet();
        }
        UIManager.Instance.gameObject.SetActive(active);
    }

    //보스가 죽을 때 넣어줄 슬로우모션 기능
    public void SlowMothion(float time, float endTime)
    {
        isSlowMotion = true;

        Time.timeScale = time;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        Invoke("EndSlowMotion", endTime * Time.unscaledDeltaTime);
    }

    void EndSlowMotion()
    {
        isSlowMotion = false;
    }
}