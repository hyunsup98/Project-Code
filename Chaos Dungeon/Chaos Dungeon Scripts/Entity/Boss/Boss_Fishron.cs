using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class Boss_Fishron : Boss
{
    //Patern_One - 보스 주위를 회전한 후 발사하는 투사체 4개 생성
    public GameObject skill_One;

    //Pattern_Two - 반사피해 활성화 시 알려줄 아이콘 오브젝트 활성화
    public GameObject reflectionIcon;

    //Pattern_Three - 전조패턴 후 레이저 공격
    public Transform laserTrans;
    public Skill skillLaser;

    public bool isReflect = false;

    //플레이어 타게팅하는 투사체 4개 발사
    public override void Pattern_One()
    {
        base.Pattern_One();
        if(skill_One != null)
            skill_One.SetActive(true);
    }

    //5초 동안 반사피해
    public override void Pattern_Two()
    {
        base.Pattern_Two();
        if (reflectionIcon != null)
            StartCoroutine("Reflective");
    }

    //레이저 공격
    public override void Pattern_Three()
    {
        float y = Mathf.Abs(transform.position.y - player.transform.position.y);
        if(y <= 1)
        {
            base.Pattern_Three();
            StartCoroutine("Laser");
        }
    }

    
    IEnumerator Reflective()
    {
        UtilObject.PlaySound("Reflect", transform, 0.2f, 1);
        Pattern_Two_OnOff(true);
        yield return new WaitForSeconds(2f);

        Pattern_Two_OnOff(false);
        yield return new WaitForSeconds(2f);
    }

    void Pattern_Two_OnOff(bool isActive)
    {
        reflectionIcon.SetActive(isActive);
        isReflect = isActive;
    }

    IEnumerator Laser()
    {
        Pattern_Three_Set(true, 0.001f);
        animator.SetTrigger("attack_laser_before");

        Skill s = SkillManager.Get(skillLaser, laserTrans);
        s.damager = this;
        s.transform.SetParent(laserTrans);
        s.GetComponent<VolumetricLineBehavior>().EndPos = dir == 1 ? new Vector3(5, 0, 0) : new Vector3(-5, 0, 0);
        s.GetComponent<BoxCollider2D>().offset = dir == 1 ? new Vector2(2.5f, 0) : new Vector2(-2.5f, 0);

        yield return new WaitForSeconds(1f);

        Pattern_Three_Set(true, 1f);
        animator.SetTrigger("attack_laser");

        yield return new WaitForSeconds(2.5f);
        animator.SetTrigger("idle");
    }

    void Pattern_Three_Set(bool isActive, float scaleX)
    {
        if(laserTrans.gameObject.activeSelf != isActive)
            laserTrans.gameObject.SetActive(isActive);

        Vector3 vec = laserTrans.localPosition;
        vec.x = dir == 1 ? 1 : -1;
        laserTrans.localPosition = vec;

        laserTrans.localScale = new Vector3(scaleX, 1, 1);
    }

    //피해를 받을때 발동
    public override void HitEvent(EntityDamageEvent evt)
    {
        if(!isReflect)
        {
            if (mobStat.defence > evt.Damage)
                evt.Damage = 1;
            else
                evt.Damage -= mobStat.defence;
        }
        else
        {
            float damage = evt.Damage - evt.GetDamager().mobStat.defence;
            evt.GetDamager().Damage(damage, evt.GetTarget());
        }
    }
}
