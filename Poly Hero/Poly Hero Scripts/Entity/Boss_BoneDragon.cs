using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

enum BossState
{
    IDLE,
    MOVE,
    ATTACK
}

public class Boss_BoneDragon : Entity
{
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private Portal portal;

    [Header("���� ���� ���� ����")]
    private BossState bossState;
    Entity player;

    [SerializeField] private float firstHp = 50f;       //ù ���� �� ü��
    [SerializeField] private float secondHp = 200f;     //�ٽ� ��Ȱ�ϰ� �� ���� ü��
    private bool isRevival = false;                     //�ٽ� ��Ƴ� ���� �ִ��� üũ
    private bool isMove = false;                       //���� �ƾ��� ����� �������� üũ

    private bool cheat = false;     //true�� ��� ���� ����x

    [Header("���� ����� �þ߰��� ���� ����")]
    [SerializeField] private float backSight = 120f;

    [Header("Bress ������ ���� ����")]
    [SerializeField] private Transform headTrans;  //�극���� ���� �� Ʈ������
    [SerializeField] private Skill bress;          //�극�� ��ų

    [Header("������ ������ ���� ����")]            //Ʈ�������� bress��ų�� �������� ���
    [SerializeField] private Skill Bite;

    [Header("����ġ�� ������ ���� ����")]
    [SerializeField] private Transform tailTrans;
    [SerializeField] private Skill whip;

    [Header("Ÿ�Ӷ��� ���� ����")]
    [SerializeField] private PlayableDirector pdRevival;   //������ �ǻ�Ƴ��� PD

    private void Start()
    {
        bossState = BossState.IDLE;
        player = GameManager.Instance.player;
        stat.hp = stat.maxhp = firstHp;
        IsFly();

    }

    private void Update()
    {
        if(isMove && !cheat)
            Act();

        if(Input.GetKeyDown(KeyCode.P))
        {
            cheat = true;
        }
    }

    public void StartFight()
    {
        isMove = true;
        bossState = BossState.MOVE;
    }

    //������ �� ���·� ��ȯ
    public void IsFly()
    {
        rigid.useGravity = false;
        animator.SetTrigger("flyidle");
    }

    //���� �������� �� ���·� ��ȯ
    public void IsGround()
    {
        rigid.useGravity = true;
        animator.SetTrigger("groundidle");
    }

    //ù ��° ����, ������ ���� �� �ձ�
    private void Bress()
    {
        Skill sBress = CreateSkill(bress, headTrans);
        sBress.targetDir = headTrans.right * -1;
    }

    //�� ��° ����, ������
    private void BiteAttack()
    {
        CreateSkill(Bite, headTrans);
    }

    //�� ��° ����, ���� ġ��
    private void TailAttack()
    {
        CreateSkill(whip, tailTrans);
    }

    private Skill CreateSkill(Skill skill, Transform trans)
    {
        Skill sSkill = SkillManager.Instance.Get(skill, trans);
        sSkill.attackEntity = this;
        sSkill.transform.SetParent(trans);

        return sSkill;
    }

    //������, ���� ���� �ൿ
    private void Act()
    {
        if(bossState != BossState.ATTACK)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if(bossState == BossState.IDLE)
            {
                animator.SetTrigger("groundidle");
                return;
            }

            if (distance > 3.8f)
            {
                bossState = BossState.MOVE;
                animator.SetTrigger("walk");

                Vector3 dir = (player.transform.position - transform.position).normalized;

                //�̵��� �� ���� �ٶ󺸱�
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = targetRot;

                rigid.velocity = stat.speed * dir;
            }
            else
            {
                StartCoroutine(ChoiceAtatck());
            }
        }
    }

    //������ �����ϴ� �Լ�
    private IEnumerator ChoiceAtatck()
    {
        bossState = BossState.ATTACK;
        bool isAtkDone = false;

        if(!PlayerIsFront())
        {
            int ran = Random.Range(0, 10);

            if(ran < 7)
            {
                if (PlayerIsRight())
                    animator.SetTrigger("tailattacktwo");
                else
                    animator.SetTrigger("tailattack");

                isAtkDone = true;
            }
        }

        if(!isAtkDone)
        {
            yield return StartCoroutine(LookPlayerPosition(0.3f));

            int atkType = Random.Range(0, 3);

            if (atkType == 0)
                animator.SetTrigger("bress");
            else if (atkType == 1)
                animator.SetTrigger("bresstwo");
            else if (atkType == 2)
                animator.SetTrigger("attack");
        }
    }

    private IEnumerator Idle()
    {
        float delay = Random.Range(1.2f, 3f);
        bossState = BossState.IDLE;

        yield return new WaitForSeconds(delay);
        bossState = BossState.MOVE;
    }

    //�÷��̾ ���� time�ʿ� ���� �ٶ󺸱�
    private IEnumerator LookPlayerPosition(float time)
    {
        Quaternion prevRotation = transform.rotation;
        Vector3 dir = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        float timer = 0;
        animator.SetTrigger("walk");

        while(timer <= time)
        {
            transform.rotation = Quaternion.Slerp(prevRotation, lookRotation, timer / time);
            timer += Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(time);
    }

    //������� ���� �� ����� ���� �Ǵ�, true�� �����x / false�� �����
    private bool PlayerIsFront()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;

        //���� ���ϱ�
        float dot = Vector3.Dot(transform.forward, targetDir);

        //������ �̿��ؼ� ������ �÷��̾� ���� ���� ���ϱ�
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //�÷��̾ ���� �ڿ� �ְ� && �þ߰� �ȿ� �ִٸ� false
        if (angle > backSight)
            return false;

        return true;
    }

    //������ �̿��� �÷��̾ �¿� ��� �ִ��� �Ǵ�, true�� ������ / false�� ����
    private bool PlayerIsRight()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;

        Vector3 cross = Vector3.Cross(transform.up, targetDir);

        if(cross.x > 0)
            return true;

        return false;
    }


    //���� Ÿ�� ����, ����� ���� ���� ������ �ϴ� ��
    public override void AttackEvent(AtkEvent atk)
    {

    }

    public override void HitEvent(AtkEvent atk)
    {
        if (!PlayerIsFront())
        {
            atk.damageType = DamageType.Critical;
            atk.damage *= 1.5f;
        }
    }

    //�������� ������� ���� ��
    public override void Damage(float damage, Entity attacker, DamageType type)
    {
        base.Damage(damage, attacker, type);
        stat.hp = Mathf.Clamp(stat.hp, 0, stat.maxhp);
        EntityUIManager.Instance.CreateDamageText(transform, 2.5f, damage, type);
        UIManager.Instance.bossHpBar.SetHP(this);

        if (stat.hp <= 0)
        {
            StopAllCoroutines();

            if(!isRevival)
            {
                isRevival = true;
                Dead();
                mainCamera.StartSlowMotion(0.1f, 3f);
                pdRevival.Play();
            }
            else
            {
                Dead();
                mainCamera.StartSlowMotion(0.2f, 2f);

                Vector3 pos = transform.position;
                pos.y += 0.4f;

                portal.gameObject.SetActive(true);
                portal.transform.position = pos;

                StartCoroutine(Disappear());
            }
        }
    }

    public void Dead()
    {
        isMove = false;
        rigid.velocity = Vector3.zero;
        StopAllCoroutines();
        animator.SetTrigger("dead");
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

    public void Revival()
    {
        stat.hp = stat.maxhp = secondHp;
        animator.SetTrigger("revival");
    }

    public void PlaySound(string sound)
    {
        SoundManager.Instance.SetSoundString(sound, transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            rigid.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rigid.isKinematic = false;
        }
    }
}
