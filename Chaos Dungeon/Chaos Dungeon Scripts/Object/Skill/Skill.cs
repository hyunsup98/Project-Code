using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    public AudioClip sound;

    //�ߺ����� ����
    List<Entity> hitEntity = new List<Entity>();

    public float damage;
    //��ų ������
    public Entity damager;
    //���� ���� ��� Ÿ��
    public string noHitType = string.Empty;

    //��ų�� �̵� �ӵ�
    public float speed = 3;

    //velocity �ӵ���
    public float speedVelo;
    //�¿� ���Ⱚ
    public int dirX;
    //�̵���ų���� ����
    public bool isDamagerMove = false;

    //��ų�� ȸ��(������ �ѹ��� ȸ��)
    //360���� 1����
    //1000�̻��̸� 1000���� �������� ����~���� ������
    //���� ) 1020 > -20~20
    public float start_rotate;

    //��ų�� ��ȯ ��ǥ[ȸ������ �������]
    //1000�̻��̸� 1000���� �������� ����~���� ������
    //���� ) 1020 > -20~20
    public Vector2 start_offset = new Vector2(0.3f, 0);

    //��ų ��ȯ �ִ� ��Ÿ�
    public float maxDistance;

    //��ų ��Ÿ��
    public float coolTime;

    //��ų ���ӽð�
    public float time = 3;
    public float timer = 0;

    //�� ���ݽ� ��ų�� ��� �����
    public bool hitRemove = false;

    //���� �ε����� ��ų�� ��� �����
    public bool groundHitRemove = false;

    //��¡�� ��ų������ ���� ����
    public bool ChargeSkill = false;

    //��ų ���۽� �ߵ�
    public List<Skill> startSkill;

    //��ų ����� �ߵ�
    public List<Skill> removeSkill;

    //��ų ������ �ߵ�
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
        //���� ȸ��
        if (!isStart)
            SkillStart();

        //�̵�
        Move();

        //�����ð����� ��ų ���
        if (tickSkill.Count > 0)
        {
            if (tick_timer <= 0)
            {
                tick_timer = tick_time;
                SkillUpdate();
            }
            tick_timer -= GameManager.deltaTime;
        }

        //���ӽð�
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

        //RigidBody2D Velocity�� �̿��ؼ� �̵�
        VelocityMove();
    }

    //position���� �̵��� �� ���� �Լ�
    protected virtual void Move()
    {
        if (speed != 0)
            transform.position += transform.right * speed * GameManager.deltaTime;
    }

    //RigidBody�� Velocity�� �̵��� �� ���� �Լ�
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
        //����� �����ڰ� �ƴϰ� noHitType�� �ƴҶ� �ǰ�
        if (collision.gameObject.tag != noHitType && collision.gameObject != damager.gameObject)
        {
            Entity target;
            //����� ����ü�� ������ �ش� ��ų�� ���������� ������
            if (target = collision.GetComponent<Entity>())
            {
                if (!hitEntity.Contains(target))
                {
                    hitEntity.Add(target);
                    EntityDamageEvent evt = new EntityDamageEvent(damager, target, damage, AttackType.NORMAL);
                    if (!evt.Cancel)
                    {
                        target.Damage(evt.Damage, damager);
                        //true�� ���� ������ ��ų ���ŵ�
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
