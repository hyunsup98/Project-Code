using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Slime : Boss
{
    public Transform skillTrans;
    //2번 패턴에서 사용할 오브젝트: 치유 이펙트
    public Skill skill_Two;
    //3번 패턴에서 사용할 스킬: 투사체
    public Skill skill_Three;

    bool isRush = false;
    public override void Attack()
    {
        base.Attack();
    }

    //일정거리 이상일 때 돌진 패턴
    public override void Pattern_One()
    {
        base.Pattern_One();
        isRush = true;
        if (player.transform.position.y > transform.position.y + 1)
            body2d.AddForce(new Vector3(dir, 2f, 0) * 3f, ForceMode2D.Impulse);
        else
            body2d.AddForce(new Vector3(dir, 0.6f, 0) * 3f, ForceMode2D.Impulse);
        Invoke("ChangeState", 1f);

    }

    //본인 체력 회복
    public override void Pattern_Two()
    {
        base.Pattern_Two();
        if (mobStat.hp <= mobStat.max_hp - 5)
        {
            Skill effect = SkillManager.Get(skill_Two, skillTrans);
            effect.transform.SetParent(skillTrans);
            effect.damager = this;
            mobStat.hp += 20;
        }
    }

    //투사체 공격
    public override void Pattern_Three()
    {
        base.Pattern_Three();
        Skill s = SkillManager.Get(skill_Three, skillTrans);
        s.damager = this;
        s.transform.position = transform.position;
        s.dirX = dir;
    }

    void ChangeState()
    {
        isRush = false;
    }

    //죽을때 발동
    public override void DeathEvent(EntityDeathEvent evt)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") && isRush)
        {
            player.Damage(mobStat.damage, player);
        }
    }

    public override void HitEvent(EntityDamageEvent evt)
    {

    }
}
