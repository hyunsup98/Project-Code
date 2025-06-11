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

    [Header("���� ���� �� ����� ���� �� ������ �ð�")]
    //���� ���� �� �������� ������� attackDelay�� ���� �ĺ��� ����
    [SerializeField] float attackTime;
    [Header("���� �ӵ� ������ �ð�")]
    //���ݼӵ� ������
    [SerializeField] float attackDelay;
    //���ݿ��� -> false�� ��쿡�� ���� ���� ����
    protected bool isAttacking;
    //���ݹ��� Ÿ���� ���̾�
    [SerializeField] LayerMask targetMask;
    //���� ������ �� �Ÿ��ȿ� ����� ������ ����� ����
    [SerializeField] float damageDis;
    [SerializeField] float yPos;    //���� ���� y�����ǰ�

    public Vector3 originPos;
    Transform detectedPlayer = null;
    BehaviorTreeRunner BTRunner = null;

    [SerializeField] float exp;

    private HPBarUI hpBar;

    [Header("���Ͱ� �����Ǵ� ����Ʈ")]
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

    //stateName�� �ִϸ��̼��� �������̸� true��ȯ �׷��� ������ false ��ȯ
    bool IsAnimationRunning(string stateName)
    {
        if (animator != null)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                //animator.normalizedTime = �ִϸ��̼� ���� �ð��� 0~1�� ����� ��ȯ(0�� �����ϴ� ����, 1�� ������ ����)
                var normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                //normalizedTime�� 0�� �ƴϰ� 1���� ������ = �ִϸ��̼��� �������̸� true ����
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

        //�̵��� �� ���� �ٶ󺸱�
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

    //this�� �����ϴ� �Լ�
    public override void AttackEvent(AtkEvent atk)
    {

    }

    public override void HitEvent(AtkEvent atk)
    {

    }

    //this�� ����� �Դ� �Լ�
    public override void Damage(float damage, Entity attacker, DamageType type)
    {
        if (stat.hp <= 0)
            return;

        base.Damage(damage, attacker, type);
        EntityUIManager.Instance.CreateDamageText(transform, 1f, damage, type);
        animator.SetTrigger("hit");
        SoundManager.Instance.SetSound(hitSound, transform);
        //hitEvent�� null�� �ƴϸ� ����
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

    //�װ� ���� �����ð�(disappearTime) �� �������
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
