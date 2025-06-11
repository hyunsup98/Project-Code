using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 기본 변수")]
    public string sName;            //스킬 이름
    public Sprite sIcon;            //스킬 아이콘
    public float sDamage;           //스킬 대미지
    public float sRange;            //스킬 사거리
    public float sCooltime;         //스킬 쿨타임
    public float sSpeed;            //스킬 이동속도
    public DamageType sDamageType;  //스킬 대미지타입
    public int sLevel = 1;          //스킬을 사용하기 위해 요구되는 플레이어 레벨
    [Multiline]
    public string sInfo;            //스킬 설명

    //스킬 시전 엔티티
    public Entity attackEntity;

    //스킬이 이동할 방향
    public Vector3 targetDir = Vector3.zero;
    //스킬이 처음 생성되는 포지션 + dir
    [SerializeField] private Vector3 dir;
    //스킬의 초기 로테이션 값
    public Vector3 rot;

    [Header("sTime(초)만큼 스킬 유지")]
    //지속시간, sTime초만큼 지속
    public float sTime = 5;
    private float sTimer;

    //중복 공격을 방지하기 위한 리스트
    private List<Entity> hitList = new List<Entity>();

    //스킬 시작시 발동될 스킬
    public List<Skill> startSkill;
    //스킬 소멸시 발동될 스킬
    public List<Skill> endSkill;

    [Header("entity 피격시 스킬 사라짐")]
    //생명체 피격시 사라짐
    public bool entityHitRemove = false;
    [Header("땅에 피격시 스킬 사라짐")]
    //땅과 닿으면 사라짐
    public bool groundHitRemove = false;
    [Header("true 체크시 range만큼의 범위공격")]
    //범위 공격인지, 범위 공격이 아니면 ontrigger를 이용해 즉발 공격, 범위 공격이면 일정 시간 후 자신의 위치에서 overlapSphere로 일정 범위만큼 타격
    public bool rangeAttack = false;
    //범위 값
    public float range;

    private bool isStart = false;


    void Update()
    {
        if(!isStart)
        {
            StartSkill();
        }

        Move();

        sTimer += Time.deltaTime;
        if(sTimer > sTime)
        {
            EndSkill();
            if(rangeAttack && range > 0)
            {
                RangeAttackTrue();
            }
        }
    }

    void Move()
    {
        if(sSpeed > 0)
        {
            transform.Translate(sSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    void RangeAttackTrue()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, range, 1 << LayerMask.NameToLayer("Monster"));
        if(col.Length > 0)
        {
            foreach (var e in col)
            {
                if(e.GetComponent<Entity>() != null && !e.CompareTag(attackEntity.tag))
                {
                    AtkEvent atk = new AtkEvent(attackEntity, e.GetComponent<Entity>(), sDamage, sDamageType);

                    if (!atk.isCancle)
                    {
                        e.GetComponent<Entity>().Damage(atk.damage, attackEntity, atk.damageType);
                    }
                }
            }

        }
    }

    //스킬이 시작되는 순간 실행할 함수
    void StartSkill()
    {
        if(!isStart)
        {
            isStart = true;
            transform.localPosition += dir;
            transform.forward = targetDir;

            if(rot != Vector3.zero)
                transform.localRotation = Quaternion.Euler(rot);

            InvokeSkill(startSkill);
        }
    }

    //스킬이 사라질 때 실행할 함수
    void EndSkill()
    {
        InvokeSkill(endSkill);
        SkillManager.Instance.Take(this);
    }

    //매개변수로 받은 스킬들을 시전
    void InvokeSkill(List<Skill> skills)
    {
        if(skills.Count > 0)
        {
            foreach(Skill s in skills)
            {
                Skill skill = SkillManager.Instance.Get(s, transform);
                skill.attackEntity = attackEntity;
                skill.dir = dir;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //스킬을 시전한 대상의 태그와 다르고 IDamageable 인터페이스를 상속하고 있다면
        if(!rangeAttack && !other.CompareTag(attackEntity.tag) && other.GetComponent<IDamageable>() != null)
        {
            //충돌체가 Entity라면
            if(other.GetComponent<Entity>() != null)
            {
                Entity damager = other.GetComponent<Entity>();

                if (!hitList.Contains(damager))
                {
                    hitList.Add(damager);

                    AtkEvent atk = new AtkEvent(attackEntity, damager, sDamage, sDamageType);

                    if(!atk.isCancle)
                    {
                        damager.Damage(atk.damage, attackEntity, atk.damageType);

                        if(entityHitRemove)
                        {
                            EndSkill();
                        }
                    }
                }
            }
        }

        if(other.CompareTag("Ground") || other.CompareTag("Structure"))
        {
            if(groundHitRemove)
            {
                EndSkill();
            }
            else
            {
                sSpeed = 0;
            }
        }
    }

    private void OnEnable()
    {
        isStart = false;
        sTimer = 0;
        hitList.Clear();
    }
}
