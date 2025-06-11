using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    [SerializeField] private QuestSlot questSlot;
    [SerializeField] private List<Quest> questList = new List<Quest>();
    [SerializeField] private QuestInfo questInfo;

    private void Start()
    {
        if(questList.Count > 0)
        {
            foreach(Quest quest in questList)
            {
                QuestSlot qs = Instantiate(questSlot, transform);
                qs.quest = quest;
                quest.slot = qs;
                qs.questInfo = questInfo;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        GameManager.Instance.SetGameState(gameObject, false);
    }
}
