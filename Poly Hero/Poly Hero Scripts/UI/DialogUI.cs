using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public TMP_Text lines;
    public TMP_Text npcName;

    public Image nextLineImg;

    List<string> listLines = new List<string>();
    NPC eventNpc;

    int index;          //재생중인 대사 라인 인덱스
    int eventIndex;     //이벤트 실행할 라인 인덱스

    [SerializeField] float lerpTime = 1;
    float currentTime = 0;
    int dir = 1;

    [SerializeField] float minAlpha, maxAlpha;

    private void Update()
    {
        if (nextLineImg.gameObject.activeSelf)
            SetAlpha();

        if (index < listLines.Count)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopAllCoroutines();
                StartCoroutine(TypingEffect(listLines[index]));
                index++;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            gameObject.SetActive(false);
    }

    public void SetDialog(DialogData data, NPC npc)
    {
        eventNpc = npc;

        if (eventNpc.isFirstTalk)
        {
            listLines = data.prevDialog;
            eventIndex = data.prevEventIndex;
        }
        else
        {
            listLines = data.nextDialog;
            eventIndex = data.nextEventIndex;
        }
        
        if (listLines != null)
        {
            StartCoroutine(TypingEffect(listLines[index]));
            npcName.text = $"- {npc.data.name} -";
            index++;
        }
    }

    IEnumerator TypingEffect(string _line)
    {
        lines.text = string.Empty;

        if (string.IsNullOrEmpty(_line))
            yield return null;

        for(int i = 0; i < _line.Length; i++)
        {
            lines.text += _line[i];
            yield return new WaitForSeconds(0.05f);
        }

        //eventIndex가 0보다 커야 실행, 0이하는 이벤트가 없는 것
        if (index == eventIndex && eventIndex > 0)
            eventNpc.NPCEvent();
    }

    void SetAlpha()
    {
        currentTime += dir * Time.deltaTime;

        if (nextLineImg.color.a >= maxAlpha)
            dir = -1;
        else if (nextLineImg.color.a <= minAlpha)
            dir = 1;

        Color color = nextLineImg.color;
        color.a = Mathf.Lerp(minAlpha, maxAlpha, currentTime / lerpTime);
        nextLineImg.color = color;
    }

    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        index = 0;
        currentTime = 0;
        eventNpc.isCanTalk = true;

        GameManager.Instance.SetGameState(gameObject, false);
    }
}
