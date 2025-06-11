using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public AudioClip equipWeapon;

    //인벤토리 오브젝트
    public GameObject obj_Inventory;

    //왼쪽 하단에 표시 될 주/보조무기 ui
    [SerializeField] Image img_MainWeapon;
    [SerializeField] Image img_SubWeapon;

    //차징 스킬 관련 오브젝트
    [SerializeField] ChargingSkill_Collider charge_Col;
    public GameObject Charging_Bar;
    public Image guage;
    public bool isCharge = false;

    public SkillUI skillCoolUI;

    //on / off ui들
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject miniMap;
    [SerializeField] MapView map;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject itemTrans;
    [SerializeField] GameObject moveEffect;
    public GameObject BossEntryPanel;

    //G키 입력하라는 알림 텍스트
    public GameObject inputKeyUI;
    //대화 텍스트
    public DialogUI dialogUI;
    //강화 UI
    public ReinforceUI reinforceUI;

    public SlotInfo slot_MainWeapon;
    public SlotInfo slot_SubWeapon;

    public GraphicRaycaster gr;

    Player player;

    [Space]
    [Header("플레이어 스탯창")]
    public GameObject statsUI;
    public TMP_Text txtHealth;
    public TMP_Text txtDamage;
    public TMP_Text txtSpeed;
    public TMP_Text txtDefence;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        OnOffInventory();

        if (isCharge)
            guage.fillAmount += 1.2f * Time.deltaTime;
        //게이지가 풀로 차버리면 강제로 스킬 종료
        if (guage.fillAmount == 1)
            ChargingUp();

        Charging_Bar.SetActive(isCharge);

        if(Input.GetKeyDown(KeyCode.P))
        {
            statsUI.SetActive(!statsUI.activeSelf);
            if (statsUI.activeSelf)
                ShowStatsUI();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            OnMiniMap();
        }
    }

    //i키를 눌러 인벤토리를 껐다 켰다 하기
    void OnOffInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
            obj_Inventory.SetActive(!obj_Inventory.activeSelf);
    }

    //차징 스킬 버튼을 누르고 있을 때
    public void ChargingDown()
    {
        isCharge = true;
    }

    //차징 스킬 버튼을 뗐을 때
    public bool ChargingUp()
    {
        isCharge = false;

        return charge_Col.CheckSuccess();
    }

    //r 키를 눌러 스왑할 때 ui도 바꿔주기
    public void SwapWeaponImg()
    {
        //주, 보조무기 이미지 중 하나 이상 활성화일 때만 실행
        if(img_MainWeapon.gameObject.activeSelf || img_SubWeapon.gameObject.activeSelf)
        {
            Item tmpItem = slot_MainWeapon.item;
            slot_MainWeapon.SetSlot(slot_SubWeapon.item);
            slot_SubWeapon.SetSlot(tmpItem);

            SetWeaponUI();
        }
    }

    //메인 혹은 서브무기 슬롯에 무기를 착용할 경우 왼쪽 하단에 착용중인 무기 슬롯 UI에도 표시
    public void SetWeaponUI()
    {
        SetWeaponImage(slot_MainWeapon, img_MainWeapon);
        SetWeaponImage(slot_SubWeapon, img_SubWeapon);

        //메인무기 착용 슬롯의 무기를 플레이어 손에 쥐어줌
        if (slot_MainWeapon.item)
        {
            player.playerWeapon = slot_MainWeapon.weapItem;
            slot_MainWeapon.weapItem.player = player;
        }
        else
            player.playerWeapon = null;

        player.SetWeapon();
        UtilObject.PlaySound("Equip", transform, 0.2f, 1);

        if (slot_MainWeapon != null)
            skillCoolUI.SkillUIChange();
        else
            skillCoolUI.SetColor(0);
    }

    //slotWeapon에 Item이 있으면 _imgWeapon 활성화 후 이미지를 바꿈, 없으면 비활성화
    void SetWeaponImage(SlotInfo slotWeapon, Image _imgWeapon)
    {
        if (slotWeapon.item != null)
        {
            _imgWeapon.gameObject.SetActive(true);
            _imgWeapon.sprite = slotWeapon.img_item.sprite;
        }
        else
            _imgWeapon.gameObject.SetActive(false);
    }

    public void ShowStatsUI()
    {
        Player p = GameManager.GetPlayer();

        txtHealth.text = $"체력: {p.mobStat.hp} / {p.mobStat.max_hp}";
        if (p.playerWeapon != null)
            txtDamage.text = $"공격력: {p.mobStat.damage + p.playerWeapon.itemstat.damage} (<color=orange>{p.mobStat.origin_damage}</color> + " +
                $"<color=yellow>{p.passiveDamage}</color> + <color=blue>{p.playerWeapon.itemstat.damage}</color>)";
        else
            txtDamage.text = $"공격력: {p.mobStat.damage} (<color=orange>{p.mobStat.origin_damage}</color> + <color=yellow>{p.passiveDamage}</color>)";
        txtSpeed.text = $"이동 속도: {p.mobStat.move_speed}";
        txtDefence.text = $"방어력: {p.mobStat.defence}";
    }

    public static void GameOver()
    {
        Instance.gameOver.SetActive(true);
    }

    public static void OnMiniMap()
    {
        Instance.map.gameObject.SetActive(!Instance.map.gameObject.active);
    }

    public static void OnMove()
    {
        Instance.moveEffect.SetActive(true);
    }

    public static void ReSet()
    {
        for (int i = 0; i < instance.transform.childCount; i++)
        {
            Transform child = instance.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
        instance.map.ReCreate();
        instance.playerUI.gameObject.SetActive(true);
        instance.miniMap.SetActive(true);
        instance.itemTrans.SetActive(true);
    }
}
