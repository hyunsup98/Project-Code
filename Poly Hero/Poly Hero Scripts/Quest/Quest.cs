using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum QuestType
{
    KILL,       //Ư�� ����� ���̴� ����Ʈ
    COLLECT,    //Ư�� ����� �����ϴ� ����Ʈ
    TALK        //Ư�� ���� ��ȭ�ϴ� ����Ʈ
}

public enum QuestProgress
{
    NONE,                //����Ʈ ���� ��
    DOING,               //����Ʈ ������
    SUCCESSBEFORE,       //����Ʈ �Ϸ� ��(���� �ޱ� ��)
    SUCCESSAFTER         //����Ʈ �Ϸ� ��(���� ���� ��)
}

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public QuestType type;    //� ����Ʈ Ÿ������
    public QuestProgress progress;
    public string questName;    //����Ʈâ�� ǥ�õ� ����Ʈ ����
    [Multiline]
    public string questInfo;    //����Ʈâ�� ǥ�õ� ����Ʈ ����
    public QuestSlot slot;      //���� ����Ʈ�� �Ҵ�ް� �ִ� ����Ʈ ����
    [SerializeField] private int id;            //����Ʈ�� �����ϱ� ���� ���, KILL = ���� ��� ���̵� Collect = ������ ��� ���̵� Talk = ��ȭ�� ��� ���̵�
    [SerializeField] private int requireCount;      //�󸶳� �����߰� ������ �䱸�ϴ� ��ġ
    [SerializeField] private int count = 0;         //�󸶳� �����߰� �׿����� ī����

    [SerializeField] private int rewardExp;     //����Ʈ �Ϸ� �� ������ ����ġ
    [SerializeField] private List<Item> rewardItem = new List<Item>();      //����Ʈ �Ϸ� �� ������ ������

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

    //����Ʈ�� �Ϸ�Ǹ� ������ ���� �� ���� ����
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
