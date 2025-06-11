using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Monster : Entity
{
    public Map map;

    private bool isFly = false;
    protected HpBar hp_bar;
    //우선도 높은 ai를 가장 아래로 <다른 ai가 안끊기게>
    //주변에 플레이어 없으면 돌아다닌다
    //플레이어가 주변에 있으면 다가간다
    //플레이어가 가까이 있으면 공격한다
    [SerializeField] protected List<AI> ai;
    public Animator animator;
    float hitColor = 0f;
    Vector3 size;

    // Start is called before the first frame update
    void Start()
    {
        hp_bar = UtilObject.GetHpBar(transform);
        hp_bar.transform.localPosition = new Vector3(0, -render.size.y, 0);
        isFly = body2d.gravityScale == 0 ? true : false;
        size = transform.localScale;
    }

    private void OnEnable()
    {
        isAlive = true;
        mobStat.hp = mobStat.max_hp;
        actions.Clear();
    }

    private void OnDestroy()
    {
        for (int i=0; i < transform.childCount;i++)
        {
            Skill s = transform.GetChild(i).GetComponent<Skill>();
            if(s != null)
            {
                SkillManager.Remove(s);
            }
        }
    }

    // Update is called once per frame
    protected override void Updates()
    {
        foreach (AI ai in ai)
        {
            if (!ai.Run(this))
            {
                break;
            }
        }
        if(hitColor > 0)
        {
            render.color = new Color(1,1 - hitColor, 1 - hitColor, 1);
            hitColor -= GameManager.deltaTime*3;
            if(hitColor <= 0)
            {
                render.color = new Color(1,1,1,1);
            }
        }
        hp_bar.SetHp(mobStat.hp / mobStat.max_hp);
    }

    public override void SetVelocity(Vector2 vector)
    {
        base.SetVelocity(vector);
        render.flipX = vector.x > 0 ? true : false;
    }

    protected override void Death()
    {
        base.Death();
        DarkPixelEffect effect = UtilObject.Effect(render.sprite, transform);
        effect.rander.flipX = render.flipX;
        effect.parent.flipX = render.flipX;
        effect.transform.localScale = transform.localScale;
        MonsterManager.Remove(this);
    }

    public override void Damage(float damage, Entity target)
    {
        base.Damage(damage, target);
        if(damage > 0)
        {
            hitColor = 1f;
        }
    }

    private void OnDisable()
    {
        if(map != null)
        {
            map.NextWave();
        }
    }

    public override void AttackEvent(EntityDamageEvent evt)
    {

    }

    public override void HitEvent(EntityDamageEvent evt)
    {
        evt.Damage -= mobStat.defence;

        //부식 적용
        if(evt.type == AttackType.COR)
        {
            float corrodeDamage = mobStat.defence - evt.GetDamager().mobStat.corrode > 0 ? mobStat.defence - evt.GetDamager().mobStat.corrode : 0;
            evt.Damage += corrodeDamage;
        }

        //출혈 적용
        if (evt.type == AttackType.DOT)
            StartCoroutine(bleedAttack(evt.GetDamager().mobStat.bleedDamage, evt.GetTarget()));

        //콤보샷 - comboCount가 5일때마다 타겟이 입는 대미지의 2배
        if (evt.GetDamager() == GameManager.GetPlayer() && GameManager.GetPlayer().isComboShot)
        {
            GameManager.GetPlayer().comboCount++;
            if (GameManager.GetPlayer().comboCount >= 5)
            {
                evt.Damage *= 2;
                GameManager.GetPlayer().comboCount = 0;
            }
        }
    }

    public override void DeathEvent(EntityDeathEvent evt)
    {

    }
    public bool IsFly() { return isFly; }
}
