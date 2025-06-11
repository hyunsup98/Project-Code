using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�ش� ��ġ�� �ؽ�Ʈ�� ����ִ� ������Ʈ
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

    // �ؽ�Ʈ ���ӽð��� ����Ǹ� ������Ʈ Ǯ�� �ǵ���
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
