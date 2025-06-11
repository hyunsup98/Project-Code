using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : DelayBehaviour
{
    public ItemDropTable dropTable;

    //��ƼƼ�� ����
    public MobStats mobStat = new MobStats();

    public SpriteRenderer render;
    public Rigidbody2D body2d;

    //�ٶ󺸴� ���� (����ü ��ȯ ����)
    public float lookRotate = 0;

    //true�� �ش� ������Ʈ�� �������
    public bool isAlive = false;

    protected bool downJump = false;
    [SerializeField] protected Collider2D collider;

    public virtual void Damage(float damage,Entity target)
    {
        mobStat.hp -= damage;
        UtilObject.SpawnText($"{damage.ToString("F1")}", transform, 0.5f).SetColor(Color.red);
        UtilObject.PlaySound("Hit", transform, 0.2f, 1);
        if (mobStat.hp <= 0)
        {
            EntityDeathEvent evt = new EntityDeathEvent(target, this);

            if (dropTable != null)
                dropTable.DropItems(transform);

            DeathEvent(evt);
            target.DeathEvent(evt);
            if (!evt.Cancel)
            {
                Death();
            }
        }
    }


    public virtual void SetVelocity(Vector2 vector)
    {
        body2d.velocity = vector;
    }
    public Vector3 GetVelocity()
    {
        return body2d.velocity;
    }
    protected virtual void Death()
    {
        isAlive = false;
        mobStat.hp = 0;
    }

    protected IEnumerator bleedAttack(float damage, Entity target)
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            Damage(damage, target);
        }
    }

    //������ �ߵ�
    public abstract void DeathEvent(EntityDeathEvent evt);

    //���ظ� �ٶ� �ߵ�
    public abstract void AttackEvent(EntityDamageEvent evt);

    //���ظ� ������ �ߵ�
    public abstract void HitEvent(EntityDamageEvent evt);
 
    public void DownJump()
    {
        if (downJump)
        {
            downJump = false;
            collider.isTrigger = true;
            Delay(() =>
            {
                collider.isTrigger = false;
                downJump = false;
            }, 0.4f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "One-Way")
        {
            downJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "One-Way")
        {
            downJump = false;
        }
    }
}