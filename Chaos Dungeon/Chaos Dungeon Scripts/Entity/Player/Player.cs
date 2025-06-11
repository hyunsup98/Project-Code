using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Net;
using Spine;

public class Player : Entity
{
    [SerializeField] BoxCollider2D col;

    //������ �ִϸ��̼�
    [SerializeField] SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Skeleton skeleton;

    //�÷��̾ �տ� ��� �ִ� ���� �̹���, ������ Ÿ��(Ÿ�Կ� ���� �÷��̾� ���� ��ų�� ����)
    //weaponPos = Ȱ�� ������ �ٸ� ���� �̹����� �־��� ������, bowPos = Ȱ �̹����� �־��� ������
    public SpriteRenderer weaponRenderer;
    public SpriteRenderer bowRenderer;

    //�÷��̾� ġ�� ����Ʈ, ������ Ʈ������
    public Skill healEffect;
    public Transform healTrans;

    //�÷��̾ �������� ����
    public Weapon playerWeapon;

    //���� ȿ�� ����
    public AudioClip parryingSound;
    public AudioClip healSound;
    public AudioClip hitSound;

    //��ų ��Ÿ��(��Ŭ��, ��Ŭ��)
    public float mouseLeft_CoolTime, mouseRight_CoolTime;

    public int passiveId;

    //�ٴ�üũ ����ĳ��Ʈ�� ���� ����
    [SerializeField] private BoxCollider2D col_CheckPlatform;

    //�÷��̾ ���� ������ �� �̵��� ��ų ����� �ݴ�� �ϱ� ���� ����
    int reverseDir;

    public PlayerHpBar hpBar;

    //�÷��̾� �ӵ�, ���� ����
    float moveX, jumpPower;

    //�÷��̾��� �ൿ üũ
    //isWall = �÷��̾ ���� �پ��ִ��� isWallJump = ���� ���� ���¿��� ������ �ߴ���
    bool isJump, isAttack, isDo, isWall, isWallJump, isDash;

    //�÷��̾ ��Ż�� �̿��� �� �ִ� ��Ÿ��
    float portalCooltime, portalTimer;
    public bool isUsePortal;

    [Space]
    [Header("�нú� ��ų")]
    //�нú� ��ų ���� ������
    public PassiveSkill playerPassive;

    //isCanParrying -> �и� ���� isBloodyAttack -> ���� ���� isCorrod - >�ν� ���� isLongShot -> ��Ÿ� ��� ���� isfastShot -> ���� ���� isHeal -> ġ�� ����
    public bool isCanParrying, isBleedAttack, isCorrode, isLongShot, isComboShot, isHeal;

    //��� �нú� ������� ���� ��ġ
    public float passiveDamage = 0;

    //�÷��̾ �и��� ������ �������� ��Ÿ��
    public bool isParry = false;
    public float parryTime;
    public float parryTimer = 0;

    [Space]
    [Range(0f, 1f)]
    //perHealAmount = ȸ���� �ۼ�Ʈ(0~1) perHealInvoke = �� ��ų�� �ߵ��� Ȯ��(0~1)
    public float perHealAmount, perHealInvoke;

    //��ų ��Ÿ�� ���ҷ�
    public float minusCoolTime = 0;

    //�޺� ���� ����
    public int comboCount = 0;

    //������ �нú� ��ų�� ������ Ÿ��
    public ItemType PassiveType;

    public int ReverseDir
    {
        get { return reverseDir; }
        set
        {
            if (value >= 0)
                value = 1;
            else
                value = -1;
            reverseDir = value;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpPower = 5.5f;
        isJump = isAttack = isDo = false;

        portalCooltime = 3f;
        isUsePortal = true;

        mobStat.origin_damage = mobStat.damage;

        mouseLeft_CoolTime = mouseRight_CoolTime = 0;
        ReverseDir = 1;
        isAlive = true;

        //������ �нú� ����
        playerPassive.PlayerPassive();
    }

    // Update is called once per frame
    protected override void Updates()
    {
        if (!isAlive) return;

        //������ Play ������ ���� ����
        if (GameManager.Instance.state == GameState.Play)
        {
            //RŰ ������ ���� ����
            if (Input.GetKeyDown(KeyCode.R))
                SwapWeapon();
            //����Ʈ �뽬
            if (hpBar.dashgasy > 0 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector3 vtr = Vector3.Normalize(ClickCheckWall()) * 7;
                vtr.z = 0;
                SetVelocity(vtr);
                hpBar.SetDashCount(hpBar.dashgasy-1);
                isJump = true;
                isDash = true;
                UtilObject.PlaySound("barray", transform, 1, 2);
                Delay(() =>
                {
                    isDash = false;
                }, 0.5f);
            }

            if (playerWeapon != null)
            {
                if (mouseLeft_CoolTime <= 0)
                {
                    if (playerWeapon.firstSkill.ChargeSkill)
                    {
                        //��¡��ų�� ��� ��Ŭ��
                        if (Input.GetMouseButtonDown(0))
                            UIManager.Instance.ChargingDown();

                        //��¡��ų�� ��� ��Ŭ�� �� ��
                        if (Input.GetMouseButtonUp(0) && UIManager.Instance.ChargingUp())
                            AttackSkill(true);
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                            AttackSkill(true);
                    }
                }

                if (mouseRight_CoolTime <= 0)
                {
                    if (playerWeapon.secondSkill.ChargeSkill)
                    {
                        //��¡��ų�� ��� ��Ŭ��
                        if (Input.GetMouseButtonDown(1))
                            UIManager.Instance.ChargingDown();

                        //��¡��ų�� ��� ��Ŭ�� �� ��
                        if (Input.GetMouseButtonUp(1) && UIManager.Instance.ChargingUp())
                            AttackSkill(false);
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse1))
                            AttackSkill(false);
                    }
                }
            }
        }

        //���콺 ��, ��Ŭ�� ��Ÿ�� �ð� ����
        if (mouseLeft_CoolTime > 0)
            mouseLeft_CoolTime -= Time.deltaTime;
        else if(mouseLeft_CoolTime < 0)
            mouseLeft_CoolTime = 0;

        if (mouseRight_CoolTime > 0)
            mouseRight_CoolTime -= Time.deltaTime;
        else if(mouseLeft_CoolTime < 0)
            mouseLeft_CoolTime = 0;

        //���� ����
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            Jump();

        //��Ż �̵� ��Ÿ�� ������
        if (!isUsePortal)
        {
            portalTimer += Time.deltaTime;
            if (portalTimer > portalCooltime)
            {
                portalTimer = 0;
                isUsePortal = true;
            }
        }

        //�и� ���� �Լ�
        if (isParry)
        {
            parryTimer += Time.deltaTime;
            if (parryTimer > parryTime)
            {
                parryTimer = 0;
                isParry = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isAlive && !isDash) { 
            if (!isWallJump)
                moveX = Input.GetAxisRaw("Horizontal") * mobStat.move_speed;

            SetVelocity(new Vector2(moveX * ReverseDir, body2d.velocity.y));

            CheckPlatform();
        }
    }

    int dir;

    //����Ű�� ������ �� ����
    void Jump()
    {
        if (downJump && Input.GetKey(KeyCode.S))
        {
            SetAnimation("jump2", true);
            DownJump();
        }
        else if (isWall)
        {
            dir = moveX > 0 ? -1 : moveX < 0 ? 1 : 0;
            isWallJump = true;
            SetVelocity(new Vector2(dir, 0.9f) * jumpPower);
            Invoke("freezeMove", 0.3f);
        }
        else
        {
            SetVelocity(Vector2.up * jumpPower);
            SetAnimation("jump", false);
            Delay(() =>
            {
                SetAnimation("jump2", true);
            }, 0.167f);
        }
        isJump = true;
    }

    void freezeMove()
    {
        isWallJump = false;
    }

    //�÷��̾� �̵��� ���õ� ���(�������� �Լ�)
    public override void SetVelocity(Vector2 vector)
    {
        if (!isAlive) return;
        base.SetVelocity(vector);

        //���߿����� ���� �ִϸ��̼Ǹ� �����ؾ��ϱ� ����

        if (vector.x != 0 && !isDo)
        {
            //�÷��̾ �̵��ϴ� ���⿡ ���� �̹����� ����
            skeleton.ScaleX = vector.x > 0 ? 1 : -1;
            if (!isJump && !isAttack)
                SetAnimation("run", true);
        }
        else if (!isJump && !isAttack)
        {
            SetAnimation("etc", true);
        }
    }

    //����ĳ��Ʈ�� �̿��ؼ� �÷��̾ ���� ����ִ��� Ȯ��
    void CheckPlatform()
    {
        int x = moveX > 0 ? 1 : moveX < 0 ? -1 : 0;
        isWall = Physics2D.Raycast(transform.position, new Vector2(x, 0), 0.2f, LayerMask.GetMask("Platform"));
        if (isWall && !isWallJump)
            body2d.velocity = Vector2.zero;

        if (body2d.velocity.y <= 0)
        {
            int layerMask = (-1) - ((1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Player")));

            RaycastHit2D raycasthit = Physics2D.BoxCast(col_CheckPlatform.bounds.center, col_CheckPlatform.bounds.size, 0f, Vector2.down, 0.02f, layerMask);
            if (raycasthit.collider != null || isWall)
                isJump = false;
            else
                isJump = true;
        }
    }

    //������ �ִϸ��̼��� �����ϴ� ���
    void SetAnimation(string animName, bool loop)
    {
        if (spineAnimationState.ToString() == animName)
            return;
        else
            spineAnimationState.SetAnimation(0, animName, loop);
    }

    //���� �����ϱ�
    void SwapWeapon()
    {
        UIManager.Instance.SwapWeaponImg();
    }

    //�������� ���� Ÿ���� Ȱ�� ��� ������ ������Ʈ���� ������(�տ� �涧 ��ġ, ������ �ٸ��� ����)
    public void SetWeapon()
    {
        if (playerWeapon == null)
        {
            weaponRenderer.sprite = null;
            bowRenderer.sprite = null;

            return;
        }

        if (playerWeapon.itemstat.type == ItemType.BOW)
        {
            weaponRenderer.sprite = null;

            bowRenderer.sprite = playerWeapon.render.sprite;
            playerWeapon.SetTrans(bowRenderer.transform);
        }
        else if (playerWeapon.itemstat.type != ItemType.ETC)
        {
            bowRenderer.sprite = null;

            weaponRenderer.sprite = playerWeapon.render.sprite;
            playerWeapon.SetTrans(weaponRenderer.transform);
        }
    }

    IEnumerator Attack(Vector3 mousePos)
    {
        if (playerWeapon != null)
        {
            isDo = isAttack = true;

            //���콺�� Ŭ���� �������� ����
            skeleton.ScaleX = mousePos.x > 0 ? 1 : -1;

            switch (playerWeapon.itemstat.type)
            {
                case ItemType.SWORD:
                    SetAnimation("attack_sword", false);
                    break;
                case ItemType.SHORT_SWORD:
                    SetAnimation("attack_small_sword", false);
                    break;
                case ItemType.BOW:
                    SetAnimation("attack_bow", false);
                    break;
                case ItemType.MAGIC:
                    SetAnimation("attack_magic", false);
                    break;
            }
            yield return new WaitForSeconds(0.4f);
            isDo = isAttack = false;
        }
    }

    //��ų ����
    void AttackSkill(bool isLeft)
    {
        if (CheckPassiveWeapon() && isCanParrying)
            isParry = true;

        Vector3 mousePos = ClickCheckWall();
        StartCoroutine(Attack(mousePos));

        if (isLeft)
            playerWeapon.LeftSkill(mousePos, minusCoolTime);
        else
            playerWeapon.RightSkill(mousePos, minusCoolTime);
    }

    //Ŭ�� ������ ���̸� ���� �ƴ� ������ ��ǥ ����
    Vector3 ClickCheckWall()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;
        direction.x *= ReverseDir;
        direction.y *= ReverseDir;

        float dis = Vector3.Distance(mousePos, transform.position);

        direction = RayUtil.CheckPlatform(transform.position, direction, dis);

        return direction;
    }

    //������ �нú� ��ų �ߵ� ����� ���� ��� �ִ� ���⸦ �� �� bool�� ��ȯ
    public bool CheckPassiveWeapon()
    {
        if (PassiveType == playerWeapon.itemstat.type)
            return true;
        return false;
    }

    //�ǰݽ� ���� Ȯ���� ������� ���� �κ� ȸ���ϴ� �нú� ��ų
    void HitHeal(float damage, float perHeal)
    {
        mobStat.hp += Mathf.Round(damage * perHeal);
        if (mobStat.hp > mobStat.max_hp)
            mobStat.hp = mobStat.max_hp;
        Skill heal = SkillManager.Get(healEffect, healTrans);
        heal.transform.SetParent(healTrans);
        heal.damager = this;

        UtilObject.PlaySound("heal", transform, 0.2f, 1);
    }

    public void Reset() {
        mobStat.hp = mobStat.max_hp;
        hpBar.SetHp(1);
        isAlive = true;
    }

    public override void AttackEvent(EntityDamageEvent evt)
    {
        playerWeapon.AttackEvent(evt);

        //�������� ����
        if (CheckPassiveWeapon() && isBleedAttack)
            evt.type = AttackType.DOT;
        //�νĵ��� ����
        if (CheckPassiveWeapon() && isCorrode)
            evt.type = AttackType.COR;
        //�̺�Ʈ �߻� ������ �÷��̾�� ������ �Ÿ��� ��� �� �Ÿ���ŭ ����� ����
        if (CheckPassiveWeapon() && isLongShot)
        {
            float disDamage = Vector2.Distance(evt.GetTarget().transform.position, evt.GetDamager().transform.position);
            disDamage = Mathf.Round(disDamage * 10) * 0.1f;
            evt.Damage += disDamage;
        }
    }

    public override void HitEvent(EntityDamageEvent evt)
    {
        evt.Damage -= mobStat.defence;

        if (isParry)
        {
            evt.Cancel = true;
            Debug.Log("�и�!");
            UtilObject.PlaySound("swordgard", transform, 0.2f, 1);
        }
    }

    public override void DeathEvent(EntityDeathEvent evt)
    {

    }

    public override void Damage(float damage, Entity target)
    {
        base.Damage(damage, target);

        int rand = Random.Range(0, 101);
        if (rand <= (perHealInvoke * 100))
            HitHeal(damage, perHealAmount);

        Camera.main.GetComponent<BloadShader>().rgbSplit = damage / mobStat.max_hp;
        hpBar.SetHp(mobStat.hp / mobStat.max_hp);
        float hpp = (mobStat.hp / mobStat.max_hp);
        GameManager.Instance.bgm.pitch = 1 + Mathf.Clamp(1.6f - hpp * 2, 0, 0.5f);
    }

    protected override void Death()
    {
        base.Death();
        SetAnimation("death", false);
        UIManager.GameOver();
    }

    public void ChangeSkin(string skinName)
    {
        Skeleton sk = skeleton;
        sk.Skin = sk.Data.FindSkin(skinName);
        sk.SetSlotsToSetupPose();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col_CheckPlatform.bounds.center, col_CheckPlatform.bounds.size);
    }
}
