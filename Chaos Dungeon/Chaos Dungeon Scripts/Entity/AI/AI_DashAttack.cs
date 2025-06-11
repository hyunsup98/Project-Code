using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



// �÷��̾ max~ min ���̿� �ִٸ� attack �� ��ȯ

public class AI_DashAttack : AI
{
    [SerializeField] float max_range;
    [SerializeField] float min_range;
    [SerializeField] Skill skill;
    [SerializeField] float attack_cooldown;
    [SerializeField] float attack_delay;
    [SerializeField] float attack_stun;
    [SerializeField] float power;
    float cooldown = 0;
    float stun = 0;


    public override bool Run(Monster entity)
    {
        int cancel = -1;
        if (cooldown > 0)
        {
            cancel = 1;
            cooldown -= GameManager.deltaTime;
        }

        if (stun > 0)
        {
            stun -= GameManager.deltaTime;
            cancel = 0;
            if(stun <= 0)
            {
                entity.animator.SetTrigger("Stand");
            }
            return false;
        }
        if (cancel > 0)
        {
            return cancel == 0 ? false : true;
        }

        Vector2 ploc = GameManager.GetPlayer().transform.position;
        Vector2 eloc = entity.transform.position;
        float range = Vector3.Distance(eloc, ploc);

        //����
        if (range < max_range && range > min_range) {
            stun = attack_stun;
            cooldown = attack_cooldown;
            entity.animator.SetTrigger("Attack1");
            entity.SetVelocity(Vector3.Normalize(ploc-eloc)*5 * power);
            entity.Delay(() => {
                Skill s = SkillManager.Get(skill, transform);
                s.transform.localScale = entity.transform.localScale;
                s.damager = entity;
                s.transform.parent = entity.transform;
                s.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                s.damage = entity.mobStat.damage;
                s.start_rotate = entity.lookRotate;
            }, attack_delay);

            return false;
        }
        return true;
    }
}
