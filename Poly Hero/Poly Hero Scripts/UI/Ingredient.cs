using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private Image img;
    public TMP_Text textCount;    //재련 재료 개수

    public void SetImage(Item item)
    {
        if(item == null)
        {
            SetAlpha(0);
            return;
        }
        img.sprite = item.itemSprite;
        SetAlpha(1);
    }

    void SetAlpha(int a)
    {
        Color color = img.color;
        color.a = a;
        img.color = color;
    }
}
