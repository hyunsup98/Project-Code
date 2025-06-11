using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PlayerController : Entity
{
    public static PlayerController Instance;

    //플레이어가 손에 들고 있는 도구에 따라 상태가 바뀜
    private IPlayerState playerState;
    //메인 카메라, 카메라가 보는 방향이 플레이어의 앞이 되기 위해서 받아줌
    public Transform mainCamera;
    //무기나 도구를 잡을 트랜스폼
    [SerializeField] private Transform handPos;
    List<Equip> weapons = new List<Equip>();

    //플레이어가 맨손일 때
    [SerializeField] private Equip HandTool;
    //플레이어가 손에 들고 있는 도구
    public Equip playerWeapon;

    //플레이어가 회전하는 스피드값
    [SerializeField] private float rotationSpeed = 45f;

    //퀵슬롯
    public List<Slot> quickSlots = new List<Slot>();
    public List<SkillQuickSlot> skillQuickSlots = new List<SkillQuickSlot>();

    //플레이어가 죽어있는지 살아있는지 체크
    public bool isAlive = true;

    //아이템 획득 관련 변수
    [SerializeField] private float range;
    private bool pickupActivated = false;
    private RaycastHit hitInfoItem;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text itemText;

    //점프 관련 변수
    private bool isJump;
    private int jumpCount, jumpMaxCount;
    public float jumpForce;

    //공격 관련 변수
    public bool isAttack = false;
    [SerializeField] Transform attackPos;   //공격 판정이 시작되는 곳

    //콤보공격 관련 변수
    bool isContinueCombo = false;

    //애니메이션 Weight값
    float animTimer = 1;

    //레벨 텍스트
    [SerializeField] TMP_Text text_Level;

    //죽었을 때 게임오버 UI
    [SerializeField] private GameOver gameoverUI;

    //애니메이션 블렌딩 변수
    private float idlerunratio = 0;

    private int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            UIManager.Instance.SetGold(money);
        }
    }

    public int Level
    {
        get { return stat.level; }
        set
        {
            text_Level.text = $"LV: {value}";
            stat.level = value;
            LevelUpAction?.Invoke();
        }
    }

    public float HP
    {
        get { return stat.hp; }
        set
        {
            if (value > stat.maxhp)
                value = stat.maxhp;
            else if (value < 0)
                value = 0;

            stat.hp = value;
            UIManager.Instance.SetHealthBar();
        }
    }
    public event Action LevelUpAction;
    private float maxExp = 100;
    private float exp = 0;
    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            if (exp >= maxExp)
            {
                Level++;
                value -= maxExp;
                maxExp += 100;
                Exp = value;
            }
            UIManager.Instance.SetExpBar(exp, maxExp);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        isJump = false;
        jumpMaxCount = jumpCount = 1;

        SetState(new PlayerHand());
        SetPlayerWeapon(HandTool);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public ItemDropTable droptable;

    // Update is called once per frame
    private void Update()
    {
        //애니메이션 체크
        CheckAnimation();

        if (GameManager.Instance.gameState == GameState.Stop || !isAlive)
            return;

        //점프
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            isJump = true;

        if (GameManager.Instance.gameState != GameState.Play || !isAlive)
            return;

        //마우스 좌클릭
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttack)
        {
            animTimer = 1;
            animator.SetLayerWeight(1, 1);
            playerState.Handle(animator);
        }

        //퀵슬롯에 등록된 도구를 장착
        QuickSlot();

        //플레이어 앞으로 레이캐스트를 쏴서 획득 가능한 템이 있으면 먹기
        CheckItem();
        PickItem();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.gameState == GameState.Stop || !isAlive)
            return;

        Move();
        
        if (isJump && jumpCount > 0)
        {
            Jump();
        }
        CheckIsJump();
    }

    //플레이어 이동 함수
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (!isJump)
        {
            if (x == 0 && z == 0)
            {
                AnimationBlendRatio(ref idlerunratio, 1, "idlerunratio", "IdleAndRun");
            }
            else
            {
                if (playerWeapon.GetComponent<Hand>())
                {
                    AnimationBlendRatio(ref idlerunratio, 0, "idlerunratio", "IdleAndRun");
                }
                else
                {
                    AnimationBlendRatio(ref idlerunratio, 2, "idlerunratio", "IdleAndRun");
                }
            }
        }

        //카메라가 보는 방향이 앞이 되도록 설정, TransformDirection = 오브젝트의 방향을 월드기준방향벡터로 치환
        Vector3 dir = new Vector3(x, 0, z).normalized;
        dir = mainCamera.TransformDirection(dir);

        //dir 길이가 0보다 크면(이동하는 중이라면) 플레이어의 방향을 카메라가 보는 방향을 기준으로 회전
        if (dir.sqrMagnitude > 0)
        {
            Quaternion cameraRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraRotation, rotationSpeed);
        }

        rigid.velocity = new Vector3(dir.x * stat.speed, rigid.velocity.y, dir.z * stat.speed);
    }

    private void AnimationBlendRatio(ref float ratio, float minmax, string rationame, string blendname)
    {
        ratio = Mathf.Lerp(ratio, minmax, 10f * Time.deltaTime);
        animator.SetFloat(rationame, ratio);
        animator.Play(blendname);
    }

    //플레이어 점프 함수
    void Jump()
    {
        jumpCount--;

        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isjump", true);
    }

    void CheckIsJump()
    {
        if (rigid.velocity.y < 0)
        {
            int layer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Structure"));
            RaycastHit hit;

            Vector3 boxHalfExtents = new Vector3(0.2f, 0.02f, 0.2f);

            if (Physics.BoxCast(transform.position, boxHalfExtents, Vector3.down, out hit, Quaternion.identity, 0.35f, layer))
            {
                if (hit.collider != null)
                {
                    animator.SetBool("isjump", false);
                    jumpCount = jumpMaxCount;
                    isJump = false;
                }
            }
        }
    }

    //0, 1번 레이어 애니메이션이 자연스럽게 합쳐지도록 하기
    void CheckAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
        {
            if (animTimer >= 0)
            {
                animTimer -= Time.deltaTime;
            }
            animator.SetLayerWeight(1, animTimer);
        }
    }

    //숫자키를 누르면 해당 숫자에 알맞은 퀵슬롯의 도구를 손에 들기
    void QuickSlot()
    {
        if (!isAttack)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PressQuickSlotsItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PressQuickSlotsItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PressQuickSlotsItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PressQuickSlotsItem(3);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                PressQuickSlotsSkill(0);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                PressQuickSlotsSkill(1);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                PressQuickSlotsSkill(2);
            }
        }
    }

    //퀵슬롯 번호키를 누를 때 아이템이 있으면 손에 들기
    public void PressQuickSlotsItem(int index)
    {
        if (quickSlots[index].item == null)
        {
            SetPlayerWeapon(HandTool);
            return;
        }

        Item item = quickSlots[index].item;

        //이미 플레이어가 들고 있는 무기일 경우
        if (playerWeapon != null)
            if (playerWeapon == item)
                return;


        if (item.itemstats.itemType == ItemType.HandEquip)
        {
            //이미 들어본 무기 리스트에 있다면 리스트에서 제거하고 현재 무기를 추가(퀵슬롯에서 같은 종류의 도구를 다른 퀵슬롯에 지정해놓고 사용할 수 있기 때문)
            foreach (var weapon in weapons)
            {
                if (weapon == item)
                {
                    SetPlayerWeapon(weapon);
                    return;
                }
                else if(weapon.itemstats.id == item.itemstats.id)
                {
                    weapons.Remove(weapon);
                    break;
                }
            }
            //해당 인덱스의 퀵슬롯이 손에 드는 도구일 경우 손에 들기
            Equip weaponItem = item.GetComponent<Equip>();
            weaponItem.transform.SetParent(handPos);
            SetPlayerWeapon(weaponItem);
            weapons.Add(weaponItem);
        }
        else if (item.itemstats.itemType == ItemType.Use && quickSlots[index].CoolTime <= 0)
        {
            //item이 IUseable 인터페이스를 상속받고 있다면(null이 아니라면) Use실행
            item.GetComponent<IUseable>()?.Use(this, quickSlots[index]);
        }
    }

    //스킬 퀵슬롯 눌렀을 때
    public void PressQuickSlotsSkill(int index)
    {
        if (skillQuickSlots[index].skill == null || skillQuickSlots[index].CoolTime > 0)
            return;

        Skill s = SkillManager.Instance.Get(skillQuickSlots[index].skill, transform);
        s.attackEntity = this;
        Vector3 skillDir = mainCamera.transform.forward;
        transform.forward = s.targetDir = new Vector3(skillDir.x, 0, skillDir.z).normalized;
        skillQuickSlots[index].CoolTime = s.sCooltime;
    }

    public void RemoveTool()
    {
        SetPlayerWeapon(HandTool);
    }

    void SetPlayerWeapon(Equip weapon)
    {
        //기존에 들고 있던 도구가 있다면 꺼줌
        if (playerWeapon != null)
            playerWeapon.gameObject.SetActive(false);

        playerWeapon = weapon;
        playerWeapon.SetPosition();
        SetWeaponState(playerWeapon);
        weapon.player = this;
        playerWeapon.gameObject.SetActive(true);
    }

    void SetState(IPlayerState state)
    {
        playerState = state;
    }

    void SetWeaponState(Equip weapon)
    {
        switch (weapon.stats.weapontype)
        {
            case WeaponType.Sword:
                SetState(new PlayerWeaponAttack());
                break;
            case WeaponType.Axe:
                SetState(new PlayerLogging());
                break;
            case WeaponType.PickAxe:
                SetState(new PlayerMining());
                break;
            case WeaponType.Hand:
                SetState(new PlayerHand());
                break;
        }
    }

    //드랍된 아이템 획득
    void PickItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickupActivated)
            {
                if (hitInfoItem.transform != null)
                {
                    Item item = hitInfoItem.transform.GetComponent<RandomItem>().item;
                    item.Count = hitInfoItem.transform.GetComponent<RandomItem>().item.Count;
                    UIManager.Instance.inventory.AddItem(item);
                    GameManager.Instance.questCollectAction?.Invoke(item);
                    Destroy(hitInfoItem.transform.gameObject);
                }
            }
        }
    }

    //스크린 화면 시점 방향에 주울 템이 있다면 pickupActivated를 true로 바꿈
    void CheckItem()
    {
        Vector3 vec = transform.position;
        vec.y += 1.8f;

        if (Physics.Raycast(vec, mainCamera.transform.forward, out hitInfoItem, range, layerMask))
        {
            if (hitInfoItem.transform.CompareTag("RandomItem"))
            {
                pickupActivated = true;
                itemText.gameObject.SetActive(true);
            }
        }
        else
        {
            pickupActivated = false;
            itemText.gameObject.SetActive(false);
        }
    }

    public void WalkSound(string sound)
    {
        if (idlerunratio < 0.6f || idlerunratio > 1.4f)
        {
            SoundManager.Instance.SetSoundString(sound, transform);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CameraManager camera = FindObjectOfType<CameraManager>();
        if(camera != null)
        {
            mainCamera = camera.transform;
            camera.target = transform;
        }
    }

    protected override void Attack()
    {
        SoundManager.Instance.SetSoundString("Whoosh", transform);
        RaycastHit[] hits = Physics.BoxCastAll(attackPos.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, Quaternion.identity, 1f, LayerMask.GetMask("Monster"));

        if (hits.Length > 0)
        {
            foreach (var mon in hits)
            {
                Entity monster = mon.transform.GetComponent<Entity>();
                float damage = playerWeapon.stats.monsterDamage + (Level - 1) * 2;
                AtkEvent atk = new AtkEvent(this, monster, damage, DamageType.Normal);
                monster.Damage(atk.damage, this, atk.damageType);
            }
        }
    }

    //콤보어택 마우스 클릭 반응 체크 함수, 애니메이션 이벤트로 적용
    void CheckStartComboAttack()
    {
        isContinueCombo = false;
        isAttack = playerWeapon.isDamage = true;
        StartCoroutine(CheckComboAttack());
    }

    //콤보 공격이 가능한 시간 내에 좌클릭시 isContinueCombo = true
    IEnumerator CheckComboAttack()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        isContinueCombo = true;
    }

    //콤보 공격을 입력하지 않았을 경우 종료
    void CheckEndComboAttack()
    {
        if (!isContinueCombo)
        {
            ComboAttackEnd();
        }
    }

    //콤보 공격이 중단될 때 실행
    void ComboAttackEnd()
    {
        animator.SetBool("weaponatk", false);
        isAttack = playerWeapon.isDamage = false;
        StopCoroutine(CheckComboAttack());
    }

    //콤보 공격 이어질 때 공격 가능한 상태로 다시 초기화
    void CanWeaponAttack()
    {
        playerWeapon.AttackReady();
    }

    private void Dead()
    {
        isAlive = false;
        animator.SetTrigger("dead");
        StartCoroutine(ShowRestartUI());
    }

    public void Revival()
    {
        if (!isAlive)
        {
            isAlive = true;
            HP = stat.maxhp;
            animator.SetTrigger("revival");
        }
    }

    private IEnumerator ShowRestartUI()
    {
        yield return new WaitForSeconds(4.5f);

        GameManager.Instance.gameState = GameState.Pause;
        gameoverUI.Enable(true, "당신은 죽었습니다");
    }

    public override void AttackEvent(AtkEvent atk)
    {
        atk.damage += stat.damage + ((playerWeapon.stats.level - 1) * 5);
    }

    public override void HitEvent(AtkEvent atk)
    {

    }

    public override void Damage(float damage, Entity attacker, DamageType type)
    {
        if (isAlive)
        {
            base.Damage(damage, attacker, type);
            UIManager.Instance.SetHealthBar();

            //카메라 흔들기, 피격 이벤트
            StartCoroutine(mainCamera.GetComponent<CameraManager>().Shake(0.05f, 0.2f));

            if (stat.hp <= 0)
            {
                Dead();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(attackPos.position, new Vector3(0.5f, 0.5f, 0.5f));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.4f, 0.04f, 0.4f));
    }
}

