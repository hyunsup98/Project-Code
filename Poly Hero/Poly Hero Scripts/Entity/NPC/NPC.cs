using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class NPC : MonoBehaviour
{
    [Header("NPC ��ܿ� ǥ�� �� ���� UI ���� ����")]
    [SerializeField] public Color uiColor;          //ui ��
    [SerializeField] public string npcName;         //ui�� �� ���ڿ�(npc �̸�)
    [SerializeField] private float posY;            //ui�� ��ġ �� ��ġ
    private NpcUI npcUI;

    public bool isCanTalk = false;
    public bool isFirstTalk = true;

    public DialogData dialogData;
    public NpcData data;

    private DialogUI dialogUI;

    [SerializeField] private bool isinteraction = false;        //��ȣ�ۿ��� ������ npc�� true
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

    //�÷��̾ NPC���� ���� �ɸ� ���� ��� ȣ��
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
