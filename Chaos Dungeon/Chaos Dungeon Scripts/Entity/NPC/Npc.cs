using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MonoBehaviour
{
    public NpcData npcData;
    [Header("몇 번째 대사에서 이벤트를 실행시킬지 입력 (음수일 경우 실행x)" +
        "\n1부터 시작")]
    public int eventLineNum;

    //g키 알림 메시지
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
