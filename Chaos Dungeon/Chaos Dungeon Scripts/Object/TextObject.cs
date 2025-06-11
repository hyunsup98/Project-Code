using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//해당 위치에 텍스트를 띄워주는 오브젝트
public class TextObject : MonoBehaviour
{
    [SerializeField] TMP_Text txt;
    public Vector3 vtr = new Vector3(0,1,0);
    float time = 0;

    public void SetText(string txt)
    {
        this.txt.text = txt;
    }
    public void SetColor(Color color)
    {
        txt.color = color;
    }
    public void SetSize(float size)
    {
        txt.fontSize = size;
    }

    public void Remove(float time)
    {
        this.time = time;
    }

    // 텍스트 지속시간이 종료되면 오브젝트 풀로 되돌림
    void Update()
    {
        transform.position += vtr * GameManager.deltaTime;
        if(time > 0)
        {
            time -= GameManager.deltaTime;
            if(time <= 0)
            {
                TextManager.Remove(this);
            }
        }
    }
}
