using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician_Avatar : Boss
{
    public Boss_Magician realBody;
    //분신이기 때문에 단순한 기본 공격만 보유
    //Pattern_One - 기본 총알 발사
    public Skill bullet;

    public override void Attack()
    {
        base.Attack();

        Pattern_One();
    }

    public override void Pattern_One()
    {
        base.Pattern_One();

        Skill s = SkillManager.Get(bullet, transform);
        s.damager = this;
        s.start_rotate = SetLookRotate(transform.position);
    }

    //죽을때 발동
    public override void DeathEvent(EntityDeathEvent evt)
    {
        gameObject.SetActive(false);
    }

    //피해를 줄때 발동
    public override void AttackEvent(EntityDamageEvent evt)
    {

    }

    //피해를 받을때 발동
    public override void HitEvent(EntityDamageEvent evt)
    {

    }

    private void OnEnable()
    {
        mobStat.hp = mobStat.max_hp;
        realBody.isCanAttack = false;
    }

    private void OnDisable()
    {
        realBody.isCanAttack = true;
    }
}
