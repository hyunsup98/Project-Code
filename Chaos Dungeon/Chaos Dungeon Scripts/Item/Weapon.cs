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

    //firstSkill = ���콺 ��Ŭ��, secondSkill = ���콺 ��Ŭ��
    public Skill firstSkill;
    public Skill secondSkill;

    //�÷��̾� ��ų UI�� ���̰� �� �̹���
    public Sprite leftUI;
    public Sprite rightUI;

    public string firstSkillSound;
    public string secondSkillSound;

    //�������� ĳ���� �տ� ������ �� �����ϰ�
    public float scaleX, scaleY, scaleZ;
    public virtual void Skill(Skill skill, Vector3 pos)
    {
        //��ų ���� �� �ʿ��� �ʱⰪ �ֱ�
        Skill s = SkillManager.Get(skill, SkillManager.Instance.transform);
        s.damage = itemstat.damage + player.mobStat.damage;
        s.start_rotate = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        s.damager = player;
        s.transform.position = player.transform.position;

        //�ִ� ��Ÿ��� ������ �ִ� ��ų�� ���
        //Ŭ���� �Ÿ��� �ִ� ��Ÿ����� �� ��� ���� ���� �ִ� ��Ÿ� ��ġ�� ��ǥ�� �ٲ���
        float dis = Vector2.Distance(s.transform.position + pos, s.transform.position);
        if (s.maxDistance > 0)
        {
            if (dis > s.maxDistance)
            {
                //�Ҽ��� �� �ڸ� ������
                float disRate = MathF.Floor(s.maxDistance / dis * 100f) / 100f;
                float moveX = MathF.Floor(pos.x * disRate * 100f) / 100f;
                float moveY = MathF.Floor(pos.y * disRate * 100f) / 100f;

                Vector3 moveVec = new Vector3(moveX, moveY, 0);
                s.transform.position += moveVec;
            }
            else
                s.transform.position += pos;
        }

        //���콺 Ŭ���� �÷��̾� ���� �¿� ������� �Ǻ� �� ����Ʈ flip�� �ٲ�
        s.GetComponent<SpriteRenderer>().flipY = pos.x < 0 ? false : true;
    }
    
    public virtual void LeftSkill(Vector3 dir, float minusCool)
    {
        //��ų�� ������ ��ġ
        Skill(firstSkill, dir);
        player.mouseLeft_CoolTime = firstSkill.coolTime - minusCool;
        SkillUI.SkillFillAmount(UIManager.Instance.skillCoolUI.firstCool);

        if(firstSkillSound != null)
            UtilObject.PlaySound(firstSkillSound, transform, 0.2f, 1);
    }

    public virtual void RightSkill(Vector3 dir, float minusCool)
    {
        //��ų�� ������ ��ġ
        Skill(secondSkill, dir);
        player.mouseRight_CoolTime = secondSkill.coolTime - minusCool;
        SkillUI.SkillFillAmount(UIManager.Instance.skillCoolUI.secondCool);

        if(secondSkillSound != null)
            UtilObject.PlaySound(secondSkillSound, transform, 0.2f, 1);
    }

    //Ʈ������ �� ����
    public void SetTrans(Transform trans)
    {
        trans.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    //�� ��ȭ�� �ɷ�ġ �÷���
    public void Reinforce()
    {


    }
}
