using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : DelayBehaviour
{
    Player p;
    public Image firstSkill;
    public Image secondSkill;

    public TMP_Text firstTxt;
    public TMP_Text secondTxt;

    public Image firstCool;
    public Image secondCool;

    float firstSkillCool, secondSkillCool;
    float firstDelay, secondDelay;
    private void Start()
    {
        p = GameManager.GetPlayer();
    }

    protected override void Updates()
    {
        CoolImage(firstCool, firstTxt, p.mouseLeft_CoolTime, firstSkillCool);
        CoolImage(secondCool, secondTxt, p.mouseRight_CoolTime, secondSkillCool);
    }

    public static void SkillFillAmount(Image image)
    {
        image.fillAmount = 1;
    }

    public void SkillUIChange()
    {
        if (p.playerWeapon == null)
            return;

        SetColor(1);
        firstSkill.sprite = p.playerWeapon.leftUI;
        secondSkill.sprite = p.playerWeapon.rightUI;

        SetDelayTime(firstTxt, ref firstDelay);
        SetDelayTime(secondTxt, ref secondDelay);

        Delay(() =>
        {
            firstSkillCool = p.playerWeapon.firstSkill.coolTime;
        }, firstDelay);

        Delay(() =>
        {
            secondSkillCool = p.playerWeapon.secondSkill.coolTime;
        }, secondDelay);
    }

    public void SetColor(float a)
    {
        Color color = new Color(255, 255, 255);
        color.a = a;

        firstSkill.color = secondSkill.color = color;
    }

    void CoolImage(Image cool, TMP_Text coolTxt, float coolTime, float maxCool)
    {
        if (cool.fillAmount > 0)
        {
            cool.fillAmount -= Time.deltaTime / maxCool;
            coolTxt.text = coolTime.ToString("F1");
        }
        else
            coolTxt.text = string.Empty;
    }

    void SetDelayTime(TMP_Text text, ref float delay)
    {
        if (text.text != string.Empty)
            delay = float.Parse(text.text);
        else
            delay = 0;
    }
}
