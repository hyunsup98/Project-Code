using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Play,       //���� �÷��� ����
    Pause,      //���� �Ͻ����� ����(ex. �κ��丮 Ű�ų� ����â ų ��)
    Stop        //���� ����(ex. �÷��̾ ���� ��)
}

public class GameManager : Singleton<GameManager>
{
    [Header("���� ���� ���� ����")]
    public GameState gameState = GameState.Play;
    private List<GameObject> StopUIList = new List<GameObject>();
    [Header("�÷��̾�")]
    public PlayerController player;

    public Action<Entity> questKillAction;
    public Action<Item> questCollectAction;
    public Action<NPC> questTalkAction;

    //StopUIList�� ���� �ִ��� �������� ���� ���� ���� ����
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
