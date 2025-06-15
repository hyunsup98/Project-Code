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
    //�÷��̾ �������� ��
    [SerializeField] Map playMap;
    //���
    public AudioSource bgm;

    [Range(0, 1)] public float soundSize = 1f;
    [Range(0, 1)] public float bgmSize = 0.15f;

    //�÷��̾�
    public Player player;
    public Player playerPrefab;
    //���� �ӵ�
    public float gameSpeed = 1;
    //���� ����
    public GameState state;

    //��޿� �´� ���⸸ �־�ΰ� ��Ȳ�� �°� �������� ����
    public List<Item> commonItems = new List<Item>();
    public List<Item> normalItems = new List<Item>();
    public List<Item> rareItems = new List<Item>();
    public List<Item> epicItems = new List<Item>();
    public List<Item> legendItems = new List<Item>();

    public UIManager ui;
    [SerializeField] UIManager uiprefab;

    //���ο��� ���ӽð�
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
    }

    //�÷��̾ ��Ż�� ���� �� �̵��� playMap�� ���� ������ �ٲ���
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

    //playmap���� ī�޶� ��������
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

    //������ ���� �� �־��� ���ο��� ���
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