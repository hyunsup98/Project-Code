using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Entity
{
    public event Action HitAction;

    [SerializeField] private ItemDropTable dropItems;

    [SerializeField] private AudioClip hitSound;

    [SerializeField] private float posYHP;

    [Header("Range")]
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float meleeAttackRange = 3f;

    [Header("공격 실행 후 대미지 입을 때 까지의 시간")]
    //공격 실행 후 실질적인 대미지를 attackDelay가 지난 후부터 입힘
    [SerializeField] float attackTime;
    [Header("공격 속도 딜레이 시간")]
    //공격속도 딜레이
    [SerializeField] float attackDelay;
    //공격여부 -> false일 경우에만 공격 가능 상태
    protected bool isAttacking;
    //공격받을 타겟의 레이어
    [SerializeField] LayerMask targetMask;
    //공격 시전시 이 거리안에 대상이 있으면 대미지 입힘
    [SerializeField] float damageDis;
    [SerializeField] float yPos;    //공격 감지 y포지션값

    public Vector3 originPos;
    Transform detectedPlayer = null;
    BehaviorTreeRunner BTRunner = null;

    [SerializeField] float exp;

    private HPBarUI hpBar;

    [Header("몬스터가 스폰되는 포인트")]
    public MonsterSpawnPoint spawnPoint;

    [SerializeField] float disappearTime = 1f;
    private void Awake()
    {
        BTRunner = new BehaviorTreeRunner(SettingBT());
        originPos = transform.position;
    }

    private void Update()
    {
        BTRunner.Operate();
    }

    INode SettingBT()
    {
        return new SelectorNode
            (
            new List<INode>()
            {
                new SequenceNode
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckMeleeAttacking),
                        new ActionNode(CheckEnemywithMeleeAttacking),
                        new ActionNode(DoMeleeAttack),
                    }
                ),
                new SequenceNode
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),
                        new ActionNode(MoveToDetectEnemy),
                    }
                ),
                new ActionNode(MoveToOriginPosition)
            }
        );
    }

    //stateName의 애니메이션이 진행중이면 true반환 그렇지 않으면 false 반환
    bool IsAnimationRunning(string stateName)
    {
        if (animator != null)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                //animator.normalizedTime = 애니메이션 진행 시간을 0~1로 만들어 반환(0이 시작하는 지점, 1이 끝나는 지점)
                var normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                //normalizedTime이 0이 아니고 1보다 작으면 = 애니메이션이 진행중이면 true 리턴
                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }

        return false;
    }

    INode.ENodeState CheckMeleeAttacking()
    {
        if(IsAnimationRunning("attack"))
        {
            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Success;
    }

    INode.ENodeState CheckEnemywithMeleeAttacking()
    {
        if(detectedPlayer != null)
        {
            if(Vector3.SqrMagnitude(detectedPlayer.position - transform.position) < meleeAttackRange)
            {
                return INode.ENodeState.ENS_Success;
            }
        }

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState DoMeleeAttack()
    {
        if (detectedPlayer != null && !isAttacking)
        {
            Attack();
            animator.SetTrigger("attack");

            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState CheckDetectEnemy()
    {
        var overlapCollders = Physics.OverlapSphere(transform.position, detectRange, LayerMask.GetMask("Player"));

        if(overlapCollders != null && overlapCollders.Length > 0)
        {
            detectedPlayer = overlapCollders[0].transform;

            return INode.ENodeState.ENS_Success;
        }

        detectedPlayer = null;

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if(detectedPlayer != null)
        {
            if(Vector3.SqrMagnitude(detectedPlayer.position - transform.position) < meleeAttackRange)
            {
                return INode.ENodeState.ENS_Success;
            }

            Move(detectedPlayer.transform.position);

            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState MoveToOriginPosition()
    {
        if(Vector3.SqrMagnitude(originPos - transform.position) < 0.5f)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            Move(originPos);
            return INode.ENodeState.ENS_Running;
        }
    }

    private void Move(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;
        dir = dir.normalized;

        //이동할 때 앞을 바라보기
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = targetRot;

        rigid.velocity = transform.forward * stat.speed;
    }

    protected override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);

        Vector3 pos = transform.position;
        pos.y += yPos;

        RaycastHit hit;
        if (Physics.Raycast(pos, transform.forward, out hit, damageDis, targetMask))
        {
            AtkEvent atk = new AtkEvent(this, hit.transform.GetComponent<Entity>(), stat.damage, DamageType.Normal);

            if(stat.hp > 0)
                hit.transform.GetComponent<Entity>().Damage(atk.damage, this, atk.damageType);
        }

        yield return new WaitForSeconds(attackDelay - attackTime);
        isAttacking = false;
    }

    //this가 공격하는 함수
    public override void AttackEvent(AtkEvent atk)
    {

    }

    public override void HitEvent(AtkEvent atk)
    {

    }

    //this가 대미지 입는 함수
    public override void Damage(float damage, Entity attacker, DamageType type)
    {
        if (stat.hp <= 0)
            return;

        base.Damage(damage, attacker, type);
        EntityUIManager.Instance.CreateDamageText(transform, 1f, damage, type);
        animator.SetTrigger("hit");
        SoundManager.Instance.SetSound(hitSound, transform);
        //hitEvent가 null이 아니면 실행
        HitAction?.Invoke();

        if (stat.hp <= 0)
        {
            dropItems?.DropItems(transform);
            animator.SetTrigger("dead");
            StartCoroutine(Disappear());

            if (spawnPoint != null && spawnPoint.gameObject.activeSelf)
            {
                spawnPoint.monster = null;
                spawnPoint.SpawnMonster();
            }

            if (attacker.GetComponent<PlayerController>() != null)
            {
                attacker.GetComponent<PlayerController>().Exp += exp;
            }

            GameManager.Instance.questKillAction?.Invoke(this);
        }
    }

    //죽고 나서 일정시간(disappearTime) 후 사라지기
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);
        HPBarManager.Instance.Take(hpBar);
        MonsterManager.Instance.Take(this);
    }

    private void OnEnable()
    {
        stat.hp = stat.maxhp;
        isAttacking = false;
        hpBar = EntityUIManager.Instance.CreateHpBar(transform, posYHP);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}
