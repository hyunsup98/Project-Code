using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogUI : MonoBehaviour
{
    public TMP_Text lines;
    public TMP_Text npcName;

    public Image nextUI;

    List<string> listLines = new();
    public Npc eventNpc;

    int index;
    int eventIndex;

    public float lerpTime = 1;

    public float currentTime = 0;
    int dir = 1;

    public float minAlpha, maxAlpha;
    private void Update()
    {
        if(nextUI.gameObject.activeSelf)
            SetAlpha();

        if(index < listLines.Count)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                //이전 코루틴을 스탑하지 않으면 lines에 글씨가 중첩됨
                StopAllCoroutines();
                StartCoroutine(TypingEffect(listLines[index]));
                index++;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDialog(string name, ref List<string> listDialog, int eventIndex, Npc npc)
    {
        listLines = listDialog;
        eventNpc = npc;

        if (eventIndex >= 0)
            this.eventIndex = eventIndex;

        if (listLines != null)
        {
            StartCoroutine(TypingEffect(listLines[index]));
            npcName.text = $"- {name} -";
            index++;
        }
    }

    //한 글자씩 타이핑되는 효과
    IEnumerator TypingEffect(string _line)
    {
        if (index + 1 == eventIndex)
            eventNpc.NPCEvent();

        lines.text = string.Empty;

        if (_line == string.Empty)
            yield return null;

        for(int i = 0; i < _line.Length; i++)
        {
            lines.text += _line[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    void SetAlpha()
    {
        currentTime += dir * Time.deltaTime;

        if (nextUI.color.a >= maxAlpha)
            dir = -1;
        else if (nextUI.color.a <= minAlpha)
            dir = 1;

        Color tmpColor = nextUI.color;
        tmpColor.a = Mathf.Lerp(minAlpha, maxAlpha, currentTime / lerpTime);
        nextUI.color = tmpColor;
    }

    private void OnDisable()
    {
        index = 0;
        currentTime = 0;
    }
}
