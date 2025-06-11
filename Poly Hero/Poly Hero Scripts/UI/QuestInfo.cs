using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;    //����Ʈ �̸� �ؽ�Ʈ
    [SerializeField] private TMP_Text questInfoText;    //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] private TMP_Text questCountText;   //�䱸 ��ġ ���� �ؽ�Ʈ
    [SerializeField] private Button btnQuest;           //��ư ���� ����Ʈ
    [SerializeField] private TMP_Text btnText;          //��ư �ؽ�Ʈ, ���� - �Ϸ� - ������ ������ ���鼭 ����

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
                btnText.text = $"����";
                btnQuest.image.raycastTarget = true;
                break;
            case QuestProgress.DOING:
                btnText.text = $"������";
                btnQuest.image.raycastTarget = false;
                break;
            case QuestProgress.SUCCESSBEFORE:
                btnText.text = $"�Ϸ�";
                btnQuest.image.raycastTarget = true;
                break;
            case QuestProgress.SUCCESSAFTER:
                btnText.text = $"�Ϸ�";
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
                    btnText.text = $"������";
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
