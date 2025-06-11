using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician : Boss
{
    //Pattern_Two - 총알 발사
    public GameObject shoot;
    //Pattern_Three - 분신 생성
    public GameObject avatar;

    //분신이 없을 땐 true, 있을 땐 false
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

    //플레이어 좌우이동 및 공격 방향 반전
    public override void Pattern_One()
    {
        base.Pattern_One();

        StartCoroutine("ReverseTarget");
    }

    //반시계 방향으로 회전하며 투사체 발사
    public override void Pattern_Two()
    {
        base.Pattern_Two();

        StartCoroutine(Shoot());
    }
    
    //분신 생성, 분신이 살아있는 동안 본체는 대미지 x
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

    //피해를 받을때 발동
    public override void HitEvent(EntityDamageEvent evt)
    {
        if (!isCanAttack)
            evt.Damage = 0;
    }
}
