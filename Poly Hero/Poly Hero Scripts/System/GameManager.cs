using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Play,       //게임 플레이 상태
    Pause,      //게임 일시정지 상태(ex. 인벤토리 키거나 설정창 킬 때)
    Stop        //게임 종료(ex. 플레이어가 죽을 때)
}

public class GameManager : Singleton<GameManager>
{
    [Header("게임 상태 관련 변수")]
    public GameState gameState = GameState.Play;
    private List<GameObject> StopUIList = new List<GameObject>();
    [Header("플레이어")]
    public PlayerController player;

    public Action<Entity> questKillAction;
    public Action<Item> questCollectAction;
    public Action<NPC> questTalkAction;

    //StopUIList에 값이 있는지 없는지에 따라서 게임 상태 변경
    public void SetGameState(GameObject obj, bool isOn)
    {
        if(isOn)
        {
            StopUIList.Add(obj);
        }
        else
        {
            if(StopUIList.Contains(obj))
            {
                StopUIList.Remove(obj);
            }
        }

        Instance.gameState = Instance.StopUIList.Count > 0 ? GameState.Pause : GameState.Play;
    }
}
