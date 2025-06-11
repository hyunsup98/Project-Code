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

    //스파인 애니메이션
    [SerializeField] SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Skeleton skeleton;

    //플레이어가 손에 들고 있는 무기 이미지, 무기의 타입(타입에 따라 플레이어 공격 스킬이 변경)
    //weaponPos = 활을 제외한 다른 무기 이미지를 넣어줄 랜더러, bowPos = 활 이미지를 넣어줄 랜더러
    public SpriteRenderer weaponRenderer;
    public SpriteRenderer bowRenderer;

    //플레이어 치유 이펙트, 생성될 트랜스폼
    public Skill healEffect;
    public Transform healTrans;

    //플레이어가 장착중인 무기
    public Weapon playerWeapon;

    //여러 효과 사운드
    public AudioClip parryingSound;
    public AudioClip healSound;
    public AudioClip hitSound;

    //스킬 쿨타임(좌클릭, 우클릭)
    public float mouseLeft_CoolTime, mouseRight_CoolTime;

    public int passiveId;

    //바닥체크 레이캐스트의 세로 길이
    [SerializeField] private BoxCollider2D col_CheckPlatform;

    //플레이어가 반전 상태일 때 이동과 스킬 사용을 반대로 하기 위한 변수
    int reverseDir;

    public PlayerHpBar hpBar;

    //플레이어 속도, 점프 위력
    float moveX, jumpPower;

    //플레이어의 행동 체크
    //isWall = 플레이어가 벽에 붙어있는지 isWallJump = 벽에 붙은 상태에서 점프를 했는지
    bool isJump, isAttack, isDo, isWall, isWallJump, isDash;

    //플레이어가 포탈을 이용할 수 있는 쿨타임
    float portalCooltime, portalTimer;
    public bool isUsePortal;

    [Space]
    [Header("패시브 스킬")]
    //패시브 스킬 관련 변수들
    public PassiveSkill playerPassive;

    //isCanParrying -> 패링 여부 isBloodyAttack -> 출혈 여부 isCorrod - >부식 여부 isLongShot -> 장거리 사격 여부 isfastShot -> 연사 여부 isHeal -> 치유 여부
    public bool isCanParrying, isBleedAttack, isCorrode, isLongShot, isComboShot, isHeal;

    //장검 패시브 대미지로 오른 수치
    public float passiveDamage = 0;

    //플레이어가 패링이 가능한 상태인지 나타냄
    public bool isParry = false;
    public float parryTime;
    public float parryTimer = 0;

    [Space]
    [Range(0f, 1f)]
    //perHealAmount = 회복될 퍼센트(0~1) perHealInvoke = 힐 스킬이 발동될 확률(0~1)
    public float perHealAmount, perHealInvoke;

    //스킬 쿨타임 감소량
    public float minusCoolTime = 0;

    //콤보 관리 변수
    public int comboCount = 0;

    //선택한 패시브 스킬의 아이템 타입
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

        //선택한 패시브 적용
        playerPassive.PlayerPassive();
    }

    // Update is called once per frame
    protected override void Updates()
    {
        if (!isAlive) return;

        //게임이 Play 상태일 때만 동작
        if (GameManager.Instance.state == GameState.Play)
        {
            //R키 누르면 무기 스왑
            if (Input.GetKeyDown(KeyCode.R))
                SwapWeapon();
            //쉬프트 대쉬
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
                        //차징스킬일 경우 좌클릭
                        if (Input.GetMouseButtonDown(0))
                            UIManager.Instance.ChargingDown();

                        //차징스킬일 경우 좌클릭 뗄 때
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
                        //차징스킬일 경우 우클릭
                        if (Input.GetMouseButtonDown(1))
                            UIManager.Instance.ChargingDown();

                        //차징스킬일 경우 우클릭 뗄 때
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

        //마우스 좌, 우클릭 쿨타임 시간 동작
        if (mouseLeft_CoolTime > 0)
            mouseLeft_CoolTime -= Time.deltaTime;
        else if(mouseLeft_CoolTime < 0)
            mouseLeft_CoolTime = 0;

        if (mouseRight_CoolTime > 0)
            mouseRight_CoolTime -= Time.deltaTime;
        else if(mouseLeft_CoolTime < 0)
            mouseLeft_CoolTime = 0;

        //점프 실행
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            Jump();

        //포탈 이동 쿨타임 돌리기
        if (!isUsePortal)
        {
            portalTimer += Time.deltaTime;
            if (portalTimer > portalCooltime)
            {
                portalTimer = 0;
                isUsePortal = true;
            }
        }

        //패링 관련 함수
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

    //점프키를 눌렀을 때 실행
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

    //플레이어 이동과 관련된 기능(재정의한 함수)
    public override void SetVelocity(Vector2 vector)
    {
        if (!isAlive) return;
        base.SetVelocity(vector);

        //공중에서는 점프 애니메이션만 구현해야하기 때문

        if (vector.x != 0 && !isDo)
        {
            //플레이어가 이동하는 방향에 따라 이미지를 반전
            skeleton.ScaleX = vector.x > 0 ? 1 : -1;
            if (!isJump && !isAttack)
                SetAnimation("run", true);
        }
        else if (!isJump && !isAttack)
        {
            SetAnimation("etc", true);
        }
    }

    //레이캐스트를 이용해서 플레이어가 땅을 밟고있는지 확인
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

    //스파인 애니메이션을 관리하는 기능
    void SetAnimation(string animName, bool loop)
    {
        if (spineAnimationState.ToString() == animName)
            return;
        else
            spineAnimationState.SetAnimation(0, animName, loop);
    }

    //무기 스왑하기
    void SwapWeapon()
    {
        UIManager.Instance.SwapWeaponImg();
    }

    //장착중인 무기 타입이 활일 경우 별도의 오브젝트에서 보여줌(손에 쥘때 위치, 각도가 다르기 때문)
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

            //마우스를 클릭한 방향으로 공격
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

    //스킬 생성
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

    //클릭 지점이 벽이면 벽이 아닌 곳으로 좌표 변경
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

    //선택한 패시브 스킬 발동 무기와 현재 들고 있는 무기를 비교 후 bool값 반환
    public bool CheckPassiveWeapon()
    {
        if (PassiveType == playerWeapon.itemstat.type)
            return true;
        return false;
    }

    //피격시 일정 확률로 대미지의 일정 부분 회복하는 패시브 스킬
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

        //출혈딜로 변경
        if (CheckPassiveWeapon() && isBleedAttack)
            evt.type = AttackType.DOT;
        //부식딜로 변경
        if (CheckPassiveWeapon() && isCorrode)
            evt.type = AttackType.COR;
        //이벤트 발생 시점에 플레이어와 몬스터의 거리를 계산 후 거리만큼 대미지 증가
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
            Debug.Log("패링!");
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
