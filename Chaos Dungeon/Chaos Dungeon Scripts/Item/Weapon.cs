using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : Item
{
    public Player player;

    //firstSkill = 마우스 좌클릭, secondSkill = 마우스 우클릭
    public Skill firstSkill;
    public Skill secondSkill;

    //플레이어 스킬 UI에 보이게 할 이미지
    public Sprite leftUI;
    public Sprite rightUI;

    public string firstSkillSound;
    public string secondSkillSound;

    //아이템이 캐릭터 손에 보여질 때 스케일값
    public float scaleX, scaleY, scaleZ;
    public virtual void Skill(Skill skill, Vector3 pos)
    {
        //스킬 생성 후 필요한 초기값 주기
        Skill s = SkillManager.Get(skill, SkillManager.Instance.transform);
        s.damage = itemstat.damage + player.mobStat.damage;
        s.start_rotate = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        s.damager = player;
        s.transform.position = player.transform.position;

        //최대 사거리를 가지고 있는 스킬일 경우
        //클릭한 거리가 최대 사거리보다 멀 경우 같은 방향 최대 사거리 위치로 좌표를 바꿔줌
        float dis = Vector2.Distance(s.transform.position + pos, s.transform.position);
        if (s.maxDistance > 0)
        {
            if (dis > s.maxDistance)
            {
                //소수점 두 자리 버리기
                float disRate = MathF.Floor(s.maxDistance / dis * 100f) / 100f;
                float moveX = MathF.Floor(pos.x * disRate * 100f) / 100f;
                float moveY = MathF.Floor(pos.y * disRate * 100f) / 100f;

                Vector3 moveVec = new Vector3(moveX, moveY, 0);
                s.transform.position += moveVec;
            }
            else
                s.transform.position += pos;
        }

        //마우스 클릭이 플레이어 기준 좌우 어디인지 판별 후 이펙트 flip을 바꿈
        s.GetComponent<SpriteRenderer>().flipY = pos.x < 0 ? false : true;
    }
    
    public virtual void LeftSkill(Vector3 dir, float minusCool)
    {
        //스킬이 생성될 위치
        Skill(firstSkill, dir);
        player.mouseLeft_CoolTime = firstSkill.coolTime - minusCool;
        SkillUI.SkillFillAmount(UIManager.Instance.skillCoolUI.firstCool);

        if(firstSkillSound != null)
            UtilObject.PlaySound(firstSkillSound, transform, 0.2f, 1);
    }

    public virtual void RightSkill(Vector3 dir, float minusCool)
    {
        //스킬이 생성될 위치
        Skill(secondSkill, dir);
        player.mouseRight_CoolTime = secondSkill.coolTime - minusCool;
        SkillUI.SkillFillAmount(UIManager.Instance.skillCoolUI.secondCool);

        if(secondSkillSound != null)
            UtilObject.PlaySound(secondSkillSound, transform, 0.2f, 1);
    }

    //트랜스폼 값 변경
    public void SetTrans(Transform trans)
    {
        trans.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    //템 강화시 능력치 올려줌
    public void Reinforce()
    {


    }
}
