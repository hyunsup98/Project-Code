using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician : Boss
{
    //Pattern_Two - �Ѿ� �߻�
    public GameObject shoot;
    //Pattern_Three - �н� ����
    public GameObject avatar;

    //�н��� ���� �� true, ���� �� false
    public bool isCanAttack = true;

    private void Start()
    {
        player = GameManager.GetPlayer();
    }

    public override void Attack()
    {
        if(isCanAttack)
            base.Attack();
    }

    //�÷��̾� �¿��̵� �� ���� ���� ����
    public override void Pattern_One()
    {
        base.Pattern_One();

        StartCoroutine("ReverseTarget");
    }

    //�ݽð� �������� ȸ���ϸ� ����ü �߻�
    public override void Pattern_Two()
    {
        base.Pattern_Two();

        StartCoroutine(Shoot());
    }
    
    //�н� ����, �н��� ����ִ� ���� ��ü�� ����� x
    public override void Pattern_Three()
    {
        base.Pattern_Three();

        avatar.SetActive(true);
    }

    IEnumerator Shoot()
    {
        body2d.gravityScale = 0;
        SetVelocity(new Vector2(0, 1.2f));

        yield return new WaitForSeconds(2f);

        SetVelocity(Vector2.zero);
        shoot.SetActive(true);

        yield return new WaitForSeconds(7f);
        body2d.gravityScale = 1;
    }

    IEnumerator ReverseTarget()
    {
        animator.SetTrigger("attack_reverse");
        player.ReverseDir = -1;

        yield return new WaitForSeconds(3f);
        animator.SetTrigger("idle");
        player.ReverseDir = 1;
    }

    //���ظ� ������ �ߵ�
    public override void HitEvent(EntityDamageEvent evt)
    {
        if (!isCanAttack)
            evt.Damage = 0;
    }
}
