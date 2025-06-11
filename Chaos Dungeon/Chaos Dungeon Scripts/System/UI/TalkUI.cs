using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkUI : MonoBehaviour
{
    //키 입력 텍스트 ui
    public TMP_Text text;

    public float lerpTime;
    public int fontMinSize, fontMaxSize;

    float currentTime;
    int a = 1;

    private void Update()
    {
        currentTime += a * Time.deltaTime;

        if (text.fontSize >= fontMaxSize)
            a = -1;
        else if (text.fontSize <= fontMinSize)
            a = 1;

        text.fontSize = Mathf.Lerp(fontMinSize, fontMaxSize, currentTime / lerpTime);
    }
}
