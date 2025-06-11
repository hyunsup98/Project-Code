using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] int maxDash = 3;
    public int dashgasy = 3;
    float dashtime = 0;
    [SerializeField] Image percent;
    [SerializeField] Transform dashTrans;
    [SerializeField] Image dash;
    [SerializeField] List<Transform> dashs;

    private void Start()
    {
        GameManager.GetPlayer().hpBar = this;
        SetDashCount(maxDash);
    }

    public void SetDashCount(int i)
    {
        dashgasy = i;
        if (dashs.Count < i)
        {
            for (int j = dashs.Count; j < i; j++)
            {
                Transform trs = Instantiate(dash, dashTrans).transform;
                trs.gameObject.SetActive(true);
                dashs.Add(trs);
            }
        }
        for(int j = 0; j < dashs.Count; j++)
        {
            dashs[j].transform.GetChild(0).gameObject.SetActive(j < i ? true : false);
        }
    }

    private void Update()
    {
        if(dashtime > 3 && dashgasy < maxDash)
        {
            dashtime = 0;
            SetDashCount(dashgasy+1);
        }
        dashtime += GameManager.deltaTime;
    }

    public void SetHp(float hp_percent)
    {
        hp_percent = Mathf.Clamp(hp_percent, 0, 1);
        percent.fillAmount = hp_percent;
    }
}
