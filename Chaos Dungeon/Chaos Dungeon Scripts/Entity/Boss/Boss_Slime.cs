using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Slime : Boss
{
    public Transform skillTrans;
    //2�� ���Ͽ��� ����� ������Ʈ: ġ�� ����Ʈ
    public Skill skill_Two;
    //3�� ���Ͽ��� ����� ��ų: ����ü
    public Skill skill_Three;

    bool isRush = false;
    public override void Attack()
    {
        base.Attack();
    }

    //�����Ÿ� �̻��� �� ���� ����
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

    //���� ü�� ȸ��
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

    //����ü ����
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

    //������ �ߵ�
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
