using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillQuickSlot : MonoBehaviour, IDropHandler
{
    public Skill skill;
    [SerializeField] Image img;

    //쿨타임이 표시될 이미지
    [SerializeField] Image coolImg;
    private float coolTime = 0;
    public float CoolTime
    {
        get { return coolTime; }

        set
        {
            coolTime = value;

            if (value > 0)
            {
                StartCoroutine(SetSlotCool(value));
            }
        }
    }

    private void Start()
    {
        UIManager.Instance.InitSlots += ClearSlot;
    }

    //슬롯 쿨타임 세팅하기
    public IEnumerator SetSlotCool(float _coolTime)
    {
        coolTime = _coolTime;

        while (coolTime > 0)
        {
            coolImg.fillAmount = coolTime / _coolTime;
            coolTime -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSkillSlot.Instance.dragslot != null)
        {
            skill = DragSkillSlot.Instance.dragslot.skill;
            img.sprite = skill.sIcon;
            SetColor(1);
        }
    }

    private void ClearSlot()
    {
        skill = null;
        img.sprite = null;
        SetColor(0);
    }

    //스킬 데이터 받기
    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        img.sprite = skill.sIcon;
        SetColor(1);
    }

    void SetColor(float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
