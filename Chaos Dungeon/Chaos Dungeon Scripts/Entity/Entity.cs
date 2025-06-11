using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : DelayBehaviour
{
    public ItemDropTable dropTable;

    //엔티티의 스탯
    public MobStats mobStat = new MobStats();

    public SpriteRenderer render;
    public Rigidbody2D body2d;

    //바라보는 방향 (투사체 소환 방향)
    public float lookRotate = 0;

    //true면 해당 오브젝트가 살아있음
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

    //죽을때 발동
    public abstract void DeathEvent(EntityDeathEvent evt);

    //피해를 줄때 발동
    public abstract void AttackEvent(EntityDamageEvent evt);

    //피해를 받을때 발동
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