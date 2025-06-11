using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MonoBehaviour
{
    public NpcData npcData;
    [Header("�� ��° ��翡�� �̺�Ʈ�� �����ų�� �Է� (������ ��� ����x)" +
        "\n1���� ����")]
    public int eventLineNum;

    //gŰ �˸� �޽���
    GameObject inputUI;
    DialogUI dialogUI;

    bool isNearPlayer = false;

    private void Start()
    {
        npcData.lines = DialogManager.Instance.GetDialog(npcData.id);
        inputUI = UIManager.Instance.inputKeyUI;
        dialogUI = UIManager.Instance.dialogUI;
        InitStart();
    }

    protected abstract void InitStart();

    private void Update()
    {
        if (!isNearPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.G))
            ShowDialog();
    }

    void ShowDialog()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUI.SetDialog(npcData.name, ref npcData.lines, eventLineNum, this);
        isNearPlayer = false;

        inputUI.gameObject.SetActive(false);
    }

    public virtual void NPCEvent()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inputUI.SetActive(true);
            isNearPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(inputUI.activeSelf)
                inputUI.SetActive(false);
            isNearPlayer = false;
        }
    }
}
