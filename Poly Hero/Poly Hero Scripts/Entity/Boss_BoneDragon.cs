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

    [Header("보스 스탯 관련 변수")]
    private BossState bossState;
    Entity player;

    [SerializeField] private float firstHp = 50f;       //첫 조우 시 체력
    [SerializeField] private float secondHp = 200f;     //다시 부활하고 난 뒤의 체력
    private bool isRevival = false;                     //다시 살아난 적이 있는지 체크
    private bool isMove = false;                       //입장 컷씬이 재생된 이후인지 체크

    private bool cheat = false;     //true일 경우 보스 동작x

    [Header("보스 백어택 시야각을 위한 변수")]
    [SerializeField] private float backSight = 120f;

    [Header("Bress 패턴을 위한 변수")]
    [SerializeField] private Transform headTrans;  //브레스가 나갈 입 트랜스폼
    [SerializeField] private Skill bress;          //브레스 스킬

    [Header("깨물기 패턴을 위한 변수")]            //트랜스폼은 bress스킬과 공통으로 사용
    [SerializeField] private Skill Bite;

    [Header("꼬리치기 패턴을 위한 변수")]
    [SerializeField] private Transform tailTrans;
    [SerializeField] private Skill whip;

    [Header("타임라인 관련 변수")]
    [SerializeField] private PlayableDirector pdRevival;   //보스가 되살아나는 PD

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

    //날았을 때 상태로 변환
    public void IsFly()
    {
        rigid.useGravity = false;
        animator.SetTrigger("flyidle");
    }

    //땅에 착지했을 때 상태로 변환
    public void IsGround()
    {
        rigid.useGravity = true;
        animator.SetTrigger("groundidle");
    }

    //첫 번째 패턴, 전방을 향해 불 뿜기
    private void Bress()
    {
        Skill sBress = CreateSkill(bress, headTrans);
        sBress.targetDir = headTrans.right * -1;
    }

    //두 번째 패턴, 깨물기
    private void BiteAttack()
    {
        CreateSkill(Bite, headTrans);
    }

    //세 번째 패턴, 꼬리 치기
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

    //움직임, 공격 등의 행동
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

                //이동할 때 앞을 바라보기
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

    //공격을 실행하는 함수
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

    //플레이어를 향해 time초에 걸쳐 바라보기
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

    //대미지를 입을 때 백어택 여부 판단, true면 백어택x / false면 백어택
    private bool PlayerIsFront()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;

        //내적 구하기
        float dot = Vector3.Dot(transform.forward, targetDir);

        //내적을 이용해서 보스와 플레이어 간의 각도 구하기
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //플레이어가 보스 뒤에 있고 && 시야각 안에 있다면 false
        if (angle > backSight)
            return false;

        return true;
    }

    //외적을 이용해 플레이어가 좌우 어디에 있는지 판단, true면 오른쪽 / false면 왼쪽
    private bool PlayerIsRight()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;

        Vector3 cross = Vector3.Cross(transform.up, targetDir);

        if(cross.x > 0)
            return true;

        return false;
    }


    //공격 타입 변경, 대미지 증가 등의 연산을 하는 곳
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

    //실질적인 대미지를 입을 때
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
