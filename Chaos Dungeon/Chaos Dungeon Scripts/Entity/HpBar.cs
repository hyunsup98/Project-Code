using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] SpriteRenderer render;
    [SerializeField] SpriteRenderer percent;

    public void SetHp(float hp_percent)
    {
        hp_percent = Mathf.Clamp(hp_percent, 0, 1);
        percent.size = new Vector2(0.74f*hp_percent,0.24f);
    }
}
