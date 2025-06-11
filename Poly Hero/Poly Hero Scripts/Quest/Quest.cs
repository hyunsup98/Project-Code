using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum QuestType
{
    KILL,       //특정 대상을 죽이는 퀘스트
    COLLECT,    //특정 대상을 수집하는 퀘스트
    TALK        //특정 대상과 대화하는 퀘스트
}

public enum QuestProgress
{
    NONE,                //퀘스트 수락 전
    DOING,               //퀘스트 진행중
    SUCCESSBEFORE,       //퀘스트 완료 후(보상 받기 전)
    SUCCESSAFTER         //퀘스트 완료 후(보상 받은 후)
}

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public QuestType type;    //어떤 퀘스트 타입인지
    public QuestProgress progress;
    public string questName;    //퀘스트창에 표시될 퀘스트 제목
    [Multiline]
    public string questInfo;    //퀘스트창에 표시될 퀘스트 설명
    public QuestSlot slot;      //현재 퀘스트를 할당받고 있는 퀘스트 슬롯
    [SerializeField] private int id;            //퀘스트를 수행하기 위한 대상, KILL = 죽일 대상 아이디 Collect = 수집할 대상 아이디 Talk = 대화할 대상 아이디
    [SerializeField] private int requireCount;      //얼마나 수집했고 죽일지 요구하는 수치
    [SerializeField] private int count = 0;         //얼마나 수집했고 죽였는지 카운팅

    [SerializeField] private int rewardExp;     //퀘스트 완료 시 지급할 경험치
    [SerializeField] private List<Item> rewardItem = new List<Item>();      //퀘스트 완료 시 지급할 아이템

    public void TypeKill(Entity entity)
    {
        if(entity.stat.id == id)
            count++;

        if(count >=  requireCount)
            progress = QuestProgress.SUCCESSBEFORE;
    }

    public void TypeCollect(Item item)
    {
        if (item.itemstats.id == id)
            count += item.Count;

        if (count >= requireCount)
            progress = QuestProgress.SUCCESSBEFORE;
    }

    public void TypeTalk(NPC npc)
    {
        if(npc.data.id == id)
        {
            progress = QuestProgress.SUCCESSBEFORE;
        }
    }

    //퀘스트가 완료되면 실행할 동작 및 보상 지급
    public void Reward()
    {
        GameManager.Instance.player.Exp += rewardExp;
        if(rewardItem.Count > 0)
        {
            foreach(Item item in rewardItem)
            {
                UIManager.Instance.inventory.AddItem(item);
            }
        }
        slot.QuestDone();
    }

    public void Init()
    {
        progress = QuestProgress.NONE;
        count = 0;
    }

    public string CountText()
    {
        if(count >= requireCount)
        {
            return $"<color=green>{requireCount}</color> / <color=green>{requireCount}</color>";
        }
        else
        {
            return $"<color=red>{count}</color> / <color=green>{requireCount}</color>";
        }
    }
}
