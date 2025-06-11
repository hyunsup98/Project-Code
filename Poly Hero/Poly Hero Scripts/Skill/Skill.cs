using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("��ų �⺻ ����")]
    public string sName;            //��ų �̸�
    public Sprite sIcon;            //��ų ������
    public float sDamage;           //��ų �����
    public float sRange;            //��ų ��Ÿ�
    public float sCooltime;         //��ų ��Ÿ��
    public float sSpeed;            //��ų �̵��ӵ�
    public DamageType sDamageType;  //��ų �����Ÿ��
    public int sLevel = 1;          //��ų�� ����ϱ� ���� �䱸�Ǵ� �÷��̾� ����
    [Multiline]
    public string sInfo;            //��ų ����

    //��ų ���� ��ƼƼ
    public Entity attackEntity;

    //��ų�� �̵��� ����
    public Vector3 targetDir = Vector3.zero;
    //��ų�� ó�� �����Ǵ� ������ + dir
    [SerializeField] private Vector3 dir;
    //��ų�� �ʱ� �����̼� ��
    public Vector3 rot;

    [Header("sTime(��)��ŭ ��ų ����")]
    //���ӽð�, sTime�ʸ�ŭ ����
    public float sTime = 5;
    private float sTimer;

    //�ߺ� ������ �����ϱ� ���� ����Ʈ
    private List<Entity> hitList = new List<Entity>();

    //��ų ���۽� �ߵ��� ��ų
    public List<Skill> startSkill;
    //��ų �Ҹ�� �ߵ��� ��ų
    public List<Skill> endSkill;

    [Header("entity �ǰݽ� ��ų �����")]
    //����ü �ǰݽ� �����
    public bool entityHitRemove = false;
    [Header("���� �ǰݽ� ��ų �����")]
    //���� ������ �����
    public bool groundHitRemove = false;
    [Header("true üũ�� range��ŭ�� ��������")]
    //���� ��������, ���� ������ �ƴϸ� ontrigger�� �̿��� ��� ����, ���� �����̸� ���� �ð� �� �ڽ��� ��ġ���� overlapSphere�� ���� ������ŭ Ÿ��
    public bool rangeAttack = false;
    //���� ��
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

    //��ų�� ���۵Ǵ� ���� ������ �Լ�
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

    //��ų�� ����� �� ������ �Լ�
    void EndSkill()
    {
        InvokeSkill(endSkill);
        SkillManager.Instance.Take(this);
    }

    //�Ű������� ���� ��ų���� ����
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
        //��ų�� ������ ����� �±׿� �ٸ��� IDamageable �������̽��� ����ϰ� �ִٸ�
        if(!rangeAttack && !other.CompareTag(attackEntity.tag) && other.GetComponent<IDamageable>() != null)
        {
            //�浹ü�� Entity���
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
