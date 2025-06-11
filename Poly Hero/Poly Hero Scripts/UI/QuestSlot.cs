using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour, IPointerClickHandler
{
    public Quest quest;
    public QuestInfo questInfo;
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private Image imgBackGround;

    private void Start()
    {
        if(quest != null)
        {
            questNameText.text = quest.questName;
            quest.Init();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            questInfo.ShowQuest(quest);
        }
    }

    //Äù½ºÆ®°¡ ¿Ï·áµÇ¸é ½½·ÔÀÇ ¹è°æ »öÀ» ¹Ù²ãÁÜ
    public void QuestDone()
    {
        imgBackGround.color = Color.green;
    }
}
