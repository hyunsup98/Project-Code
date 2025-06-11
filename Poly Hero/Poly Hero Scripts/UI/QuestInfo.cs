using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;    //퀘스트 이름 텍스트
    [SerializeField] private TMP_Text questInfoText;    //퀘스트 설명 텍스트
    [SerializeField] private TMP_Text questCountText;   //요구 수치 설명 텍스트
    [SerializeField] private Button btnQuest;           //버튼 수락 퀘스트
    [SerializeField] private TMP_Text btnText;          //버튼 텍스트, 수락 - 완료 - 진행중 번갈아 가면서 변경

    private Quest quest;

    public void ShowQuest(Quest quest)
    {
        this.quest = quest;

        questNameText.text = quest.questName;
        questInfoText.text = quest.questInfo;
        questCountText.text = quest.CountText();

        btnQuest.gameObject.SetActive(true);

        switch(quest.progress)
        {
            case QuestProgress.NONE:
                btnText.text = $"수락";
                btnQuest.image.raycastTarget = true;
                break;
            case QuestProgress.DOING:
                btnText.text = $"진행중";
                btnQuest.image.raycastTarget = false;
                break;
            case QuestProgress.SUCCESSBEFORE:
                btnText.text = $"완료";
                btnQuest.image.raycastTarget = true;
                break;
            case QuestProgress.SUCCESSAFTER:
                btnText.text = $"완료";
                btnQuest.image.raycastTarget = false;
                break;
        }
    }

    public void OnQuestAccept()
    {
        if(quest != null)
        {
            switch (quest.progress)
            {
                case QuestProgress.NONE:
                    if (quest.type == QuestType.KILL)
                    {
                        GameManager.Instance.questKillAction += quest.TypeKill;
                    }
                    else if (quest.type == QuestType.COLLECT)
                    {
                        GameManager.Instance.questCollectAction += quest.TypeCollect;
                    }
                    else if (quest.type == QuestType.TALK)
                    {
                        GameManager.Instance.questTalkAction += quest.TypeTalk;
                    }
                    quest.progress = QuestProgress.DOING;
                    btnText.text = $"진행중";
                    btnQuest.image.raycastTarget = true;
                    break;
                case QuestProgress.SUCCESSBEFORE:
                    if (quest.type == QuestType.KILL)
                    {
                        GameManager.Instance.questKillAction -= quest.TypeKill;
                    }
                    else if (quest.type == QuestType.COLLECT)
                    {
                        GameManager.Instance.questCollectAction -= quest.TypeCollect;
                    }
                    else if (quest.type == QuestType.TALK)
                    {
                        GameManager.Instance.questTalkAction -= quest.TypeTalk;
                    }
                    quest.Reward();
                    quest.progress = QuestProgress.SUCCESSAFTER;
                    break;
            }
        }
    }

    private void OnDisable()
    {
        questNameText.text = questInfoText.text = questCountText.text = string.Empty;
        btnQuest.gameObject.SetActive(false);
    }
}
