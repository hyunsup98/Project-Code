using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class NPC : MonoBehaviour
{
    [Header("NPC 상단에 표시 될 정보 UI 관련 변수")]
    [SerializeField] public Color uiColor;          //ui 색
    [SerializeField] public string npcName;         //ui에 쓸 문자열(npc 이름)
    [SerializeField] private float posY;            //ui가 배치 될 위치
    private NpcUI npcUI;

    public bool isCanTalk = false;
    public bool isFirstTalk = true;

    public DialogData dialogData;
    public NpcData data;

    private DialogUI dialogUI;

    [SerializeField] private bool isinteraction = false;        //상호작용이 가능한 npc면 true
    [SerializeField] private Collider col;

    protected void Init()
    {
        dialogUI = UIManager.Instance.dialogUI;
        SetDialogData();

        npcUI = EntityUIManager.Instance.CreateNpcUI(this, posY);
    }

    private void Update()
    {
        if(isCanTalk)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ShowDialog();
            }
        }
    }

    //플레이어가 NPC에게 말을 걸면 나올 대사 호출
    void SetDialogData()
    {
        dialogData = DialogManager.Instance.GetDialog(data.id);
    }

    void ShowDialog()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUI.SetDialog(dialogData, this);
        isCanTalk = false;

        if (isFirstTalk)
            isFirstTalk = false;
    }

    public virtual void NPCEvent()
    {

    }

    protected abstract void ExitCollider();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (isinteraction)
            {
                isCanTalk = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isinteraction)
            {
                if(dialogUI.gameObject.activeSelf)
                {
                    dialogUI.gameObject.SetActive(false);
                }
                isCanTalk = false;
                ExitCollider();
            }
        }
    }
}
