using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public AudioClip equipWeapon;

    //�κ��丮 ������Ʈ
    public GameObject obj_Inventory;

    //���� �ϴܿ� ǥ�� �� ��/�������� ui
    [SerializeField] Image img_MainWeapon;
    [SerializeField] Image img_SubWeapon;

    //��¡ ��ų ���� ������Ʈ
    [SerializeField] ChargingSkill_Collider charge_Col;
    public GameObject Charging_Bar;
    public Image guage;
    public bool isCharge = false;

    public SkillUI skillCoolUI;

    //on / off ui��
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject miniMap;
    [SerializeField] MapView map;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject itemTrans;
    [SerializeField] GameObject moveEffect;
    public GameObject BossEntryPanel;

    //GŰ �Է��϶�� �˸� �ؽ�Ʈ
    public GameObject inputKeyUI;
    //��ȭ �ؽ�Ʈ
    public DialogUI dialogUI;
    //��ȭ UI
    public ReinforceUI reinforceUI;

    public SlotInfo slot_MainWeapon;
    public SlotInfo slot_SubWeapon;

    public GraphicRaycaster gr;

    Player player;

    [Space]
    [Header("�÷��̾� ����â")]
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
        //�������� Ǯ�� �������� ������ ��ų ����
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

    //iŰ�� ���� �κ��丮�� ���� �״� �ϱ�
    void OnOffInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
            obj_Inventory.SetActive(!obj_Inventory.activeSelf);
    }

    //��¡ ��ų ��ư�� ������ ���� ��
    public void ChargingDown()
    {
        isCharge = true;
    }

    //��¡ ��ų ��ư�� ���� ��
    public bool ChargingUp()
    {
        isCharge = false;

        return charge_Col.CheckSuccess();
    }

    //r Ű�� ���� ������ �� ui�� �ٲ��ֱ�
    public void SwapWeaponImg()
    {
        //��, �������� �̹��� �� �ϳ� �̻� Ȱ��ȭ�� ���� ����
        if(img_MainWeapon.gameObject.activeSelf || img_SubWeapon.gameObject.activeSelf)
        {
            Item tmpItem = slot_MainWeapon.item;
            slot_MainWeapon.SetSlot(slot_SubWeapon.item);
            slot_SubWeapon.SetSlot(tmpItem);

            SetWeaponUI();
        }
    }

    //���� Ȥ�� ���깫�� ���Կ� ���⸦ ������ ��� ���� �ϴܿ� �������� ���� ���� UI���� ǥ��
    public void SetWeaponUI()
    {
        SetWeaponImage(slot_MainWeapon, img_MainWeapon);
        SetWeaponImage(slot_SubWeapon, img_SubWeapon);

        //���ι��� ���� ������ ���⸦ �÷��̾� �տ� �����
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

    //slotWeapon�� Item�� ������ _imgWeapon Ȱ��ȭ �� �̹����� �ٲ�, ������ ��Ȱ��ȭ
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

        txtHealth.text = $"ü��: {p.mobStat.hp} / {p.mobStat.max_hp}";
        if (p.playerWeapon != null)
            txtDamage.text = $"���ݷ�: {p.mobStat.damage + p.playerWeapon.itemstat.damage} (<color=orange>{p.mobStat.origin_damage}</color> + " +
                $"<color=yellow>{p.passiveDamage}</color> + <color=blue>{p.playerWeapon.itemstat.damage}</color>)";
        else
            txtDamage.text = $"���ݷ�: {p.mobStat.damage} (<color=orange>{p.mobStat.origin_damage}</color> + <color=yellow>{p.passiveDamage}</color>)";
        txtSpeed.text = $"�̵� �ӵ�: {p.mobStat.move_speed}";
        txtDefence.text = $"����: {p.mobStat.defence}";
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
