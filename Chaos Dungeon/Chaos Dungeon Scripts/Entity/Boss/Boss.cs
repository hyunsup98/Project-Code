using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Boss : Entity
{
    //보스 애니메이터
    public Animator animator;
    //AttackTime초 마다 공격 패턴 실행
    public float attackTimer;
    public Player player;

    //보스가 죽고 나서 나올 NPC
    public GameObject structNpc;

    [Space]
    public GameObject nextStagePortal;

    //각 패턴의 쿨타임
    public float pattern_One_Cool, pattern_Two_Cool, pattern_Three_Cool;

    [Header("0~100")]
    public int rateOne;
    public int rateTwo;

    public bool isRight;

    //움직이는 보스인지, 날아다니는 보스인지 체크
    public bool isMove, isFly, isLockPlayer = false;
    bool isAttack = false;

    //보스 공격 시 스킬 좌우 방향을 조절하기 위함
    protected int dir;

    public float moveTimer, moveTime;
    float distance;

    private void Start()
    {
        player = GameManager.GetPlayer();
    }

    protected override void Updates()
    {
        if(!isAttack && isLockPlayer)
            render.flipX = player.transform.position.x < transform.position.x ? !isRight : isRight;

        distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance < 5)
        {
            isLockPlayer = true;
            if (attackTimer <= 0)
            {
                isAttack = true;
                Attack();
            }
        }
        attackTimer -= Time.deltaTime;
        Move();
    }

    public float SetLookRotate(Vector2 pos)
    {
        Vector2 playerPos = player.transform.position;
        Vector2 bossPos = pos;
        Vector2 distance = playerPos - bossPos;

        return lookRotate = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
    }

    public void Move()
    {
        if (isMove && !isAttack)
        {
            if (moveTimer >= moveTime)
            {
                moveTimer = 0;
                moveTime = Random.Range(2, 4);
                float x, y;
                if (isLockPlayer)
                {
                    x = dir;
                    y = Mathf.Abs(player.transform.position.y - transform.position.y) < 3 ? 0.1f : -0.1f;
                }
                else
                {
                    x = Random.Range(0, 2) == 0 ? -1 : 1;
                    y = Random.Range(0, 2) == 0 ? -1 : 1;
                    render.flipX = x < 0 ? !isRight : isRight;
                }

                if(isFly)
                {
                    StartCoroutine(MoveBoss(new Vector2(x, y).normalized, moveTime));
                    
                }
                else
                {
                    StartCoroutine(MoveBoss(new Vector2(x * 2f, GetVelocity().y).normalized, moveTime));
                }
            }
        }
        moveTimer += Time.deltaTime;
    }

    IEnumerator MoveBoss(Vector2 pos, float time)
    {
        float t = Random.Range(1, time - 0.7f);
        SetVelocity(pos);
        yield return new WaitForSeconds(t);

        SetVelocity(Vector2.zero);
    }


    public virtual void Attack()
    {
        dir = player.transform.position.x > transform.position.x ? 1 : -1;

        int randPattern = Random.Range(0, 100);

        if (randPattern < rateOne)
            Pattern_One();
        else if (randPattern < rateTwo)
            Pattern_Two();
        else
            Pattern_Three();

        StartCoroutine(AttackState(attackTimer));
    }

    IEnumerator AttackState(float time)
    {
        yield return new WaitForSeconds(time - 0.2f);
        isAttack = false;
    }

    public override void Damage(float damage, Entity target)
    {
        base.Damage(damage, target);

        if(mobStat.hp <= 0)
        {
            if(nextStagePortal != null)
                nextStagePortal.gameObject.SetActive(true);
            structNpc.SetActive(true);
            GameManager.Instance.SlowMothion(0.05f, 3);
        }
    }

    public virtual void Pattern_One()
    {
        attackTimer = pattern_One_Cool;
    }
    public virtual void Pattern_Two() 
    {
        attackTimer = pattern_Two_Cool;
    }
    public virtual void Pattern_Three() 
    {
        attackTimer = pattern_Three_Cool;
    }

    //죽을때 발동
    public override void DeathEvent(EntityDeathEvent evt)
    {
        gameObject.SetActive(false);
    }

    //피해를 줄때 발동
    public override void AttackEvent(EntityDamageEvent evt)
    {
        evt.Damage *= mobStat.damage;
    }
}
