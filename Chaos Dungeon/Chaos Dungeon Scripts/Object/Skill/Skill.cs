using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    public AudioClip sound;

    //중복공격 방지
    List<Entity> hitEntity = new List<Entity>();

    public float damage;
    //스킬 시전자
    public Entity damager;
    //공격 제외 대상 타입
    public string noHitType = string.Empty;

    //스킬의 이동 속도
    public float speed = 3;

    //velocity 속도값
    public float speedVelo;
    //좌우 방향값
    public int dirX;
    //이동스킬인지 여부
    public bool isDamagerMove = false;

    //스킬의 회전(시작후 한번만 회전)
    //360도가 1바퀴
    //1000이상이면 1000빼고 나머지값 음수~정수 무작위
    //예시 ) 1020 > -20~20
    public float start_rotate;

    //스킬의 소환 좌표[회전값에 상대적임]
    //1000이상이면 1000빼고 나머지값 음수~정수 무작위
    //예시 ) 1020 > -20~20
    public Vector2 start_offset = new Vector2(0.3f, 0);

    //스킬 소환 최대 사거리
    public float maxDistance;

    //스킬 쿨타임
    public float coolTime;

    //스킬 지속시간
    public float time = 3;
    public float timer = 0;

    //적 공격시 스킬이 즉시 종료됨
    public bool hitRemove = false;

    //벽과 부딛히면 스킬이 즉시 종료됨
    public bool groundHitRemove = false;

    //차징기 스킬인지에 대한 여부
    public bool ChargeSkill = false;

    //스킬 시작시 발동
    public List<Skill> startSkill;

    //스킬 종료시 발동
    public List<Skill> removeSkill;

    //스킬 지속중 발동
    public List<Skill> tickSkill;
    public float tick_time = 0;
    float tick_timer = 0;


    bool isStart = false;
    void OnEnable()
    {
        isStart = false;
        hitEntity.Clear();
        tick_timer = 0;
        timer = time;
    }

    // Update is called once per frame
    void Update()
    {
        //시작 회전
        if (!isStart)
            SkillStart();

        //이동
        Move();

        //일정시간마다 스킬 사용
        if (tickSkill.Count > 0)
        {
            if (tick_timer <= 0)
            {
                tick_timer = tick_time;
                SkillUpdate();
            }
            tick_timer -= GameManager.deltaTime;
        }

        //지속시간
        if (timer > 0)
        {
            timer -= GameManager.deltaTime;
            if (timer <= 0)
            {
                SkillEnd();
            }
        }
        if (groundHitRemove)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.right, 0.1f, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                SkillEnd();

                if (damager == GameManager.GetPlayer())
                    GameManager.GetPlayer().comboCount = 0;
            }
        }
    }

    protected virtual void SkillStart()
    {
        if (noHitType == string.Empty)
            noHitType = damager.tag;

        if (startSkill.Count > 0)
            CastSkill(startSkill);

        isStart = true;
        transform.rotation = Quaternion.Euler(0, 0, start_rotate);

        transform.position += transform.right * start_offset.x;
        transform.position += transform.up * start_offset.y;

        //RigidBody2D Velocity를 이용해서 이동
        VelocityMove();
    }

    //position으로 이동할 때 쓰는 함수
    protected virtual void Move()
    {
        if (speed != 0)
            transform.position += transform.right * speed * GameManager.deltaTime;
    }

    //RigidBody의 Velocity로 이동할 때 쓰는 함수
    protected virtual void VelocityMove() { }

    protected virtual void SkillUpdate()
    {
        CastSkill(tickSkill);
    }

    protected virtual void SkillEnd()
    {
        if (removeSkill.Count > 0)
        {
            CastSkill(removeSkill);
        }
        SkillManager.Remove(this);
    }

    private void CastSkill(List<Skill> skill)
    {
        foreach (var sk in skill)
        {
            Skill s = SkillManager.Get(sk, transform);
            s.damager = damager;
            s.dirX = dirX;
            //s.damage = damage * damager.mobStat.damage;
            s.noHitType = noHitType;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //대상이 시전자가 아니고 noHitType이 아닐때 피격
        if (collision.gameObject.tag != noHitType && collision.gameObject != damager.gameObject)
        {
            Entity target;
            //대상이 생명체고 이전에 해당 스킬로 공격한적이 없으면
            if (target = collision.GetComponent<Entity>())
            {
                if (!hitEntity.Contains(target))
                {
                    hitEntity.Add(target);
                    EntityDamageEvent evt = new EntityDamageEvent(damager, target, damage, AttackType.NORMAL);
                    if (!evt.Cancel)
                    {
                        target.Damage(evt.Damage, damager);
                        //true이 적과 닿으면 스킬 제거됨
                        if (hitRemove)
                        {
                            SkillEnd();
                        }
                    }
                }
            }
        }
    }
}
