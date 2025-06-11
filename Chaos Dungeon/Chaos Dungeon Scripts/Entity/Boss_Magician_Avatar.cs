using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician_Avatar : Boss
{
    public Boss_Magician realBody;
    //�н��̱� ������ �ܼ��� �⺻ ���ݸ� ����
    //Pattern_One - �⺻ �Ѿ� �߻�
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

    //������ �ߵ�
    public override void DeathEvent(EntityDeathEvent evt)
    {
        gameObject.SetActive(false);
    }

    //���ظ� �ٶ� �ߵ�
    public override void AttackEvent(EntityDamageEvent evt)
    {

    }

    //���ظ� ������ �ߵ�
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
