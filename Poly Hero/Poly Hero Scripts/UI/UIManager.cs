using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject siblingObject;

    public Inventory inventory;
    public SkillUI skillUI;
    public ReinForceUI reinForceUI;
    public DialogUI dialogUI;
    public ShopUI shopUI;
    public BossHPBar bossHpBar;
    public GameOver gameover;
    public CreateTableUI createTableUI;
    public GameObject questList;

    //플레이어의 체력바
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;

    [SerializeField] private TMP_Text enterZoneTxt;
    [SerializeField] private Image enterZoneImg;

    private float currentTime;
    [SerializeField] private float onLerpTime = 1;
    [SerializeField] private float offLerpTime = 2;

    public ItemDestroyUI destroyUI;
    public ItemBuyUI itemBuyUI;

    public GraphicRaycaster gr;
    public CanvasScaler scaler;

    [Header("플레이어 보유 골드 관련 변수")]
    [SerializeField] private TMP_Text goldText;

    //구역 이름 표시해주는 코루틴
    private Coroutine showEnterZoneCoroutine;

    public event Action InitSlots;

    // Update is called once per frame
    private void Update()
    {
        InventoryOnOff();
        SkillUIOnOff();
        GameOverOnOff();
        CreateTableUIOnOff();
        QuestUIOnOff();

        if (Input.GetKeyDown(KeyCode.Escape) && siblingObject != null)
        {
            if(siblingObject.activeSelf)
            {
                siblingObject.SetActive(false);
                siblingObject = null;
            }
        }
    }

    #region UI끄고키기

    private void InventoryOnOff()
    {
        if (inventory.isOn == false)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventory.OnInventory();
                siblingObject = inventory.gameObject;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventory.OffInventory();
                siblingObject = null;
            }
        }
    }

    private void SkillUIOnOff()
    {
        if (skillUI.gameObject.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                skillUI.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                skillUI.gameObject.SetActive(false);
            }
        }
    }

    private void GameOverOnOff()
    {
        if(siblingObject == null)
        {
            if (gameover.gameObject.activeSelf == false)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameover.Enable(false, "게임 설정");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameover.gameObject.SetActive(false);
                }
            }
        }
    }

    private void CreateTableUIOnOff()
    {
        if (createTableUI.gameObject.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                createTableUI.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                createTableUI.gameObject.SetActive(false);
            }
        }
    }

    private void QuestUIOnOff()
    {
        if (questList.gameObject.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                questList.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                questList.gameObject.SetActive(false);
            }
        }
    }
#endregion

    public void ClearSlotsEvent()
    {
        InitSlots?.Invoke();
    }

    public void BossHpBar(Entity boss)
    {
        if (bossHpBar == null || bossHpBar.gameObject.activeSelf)
            return;

        bossHpBar.gameObject.SetActive(true);
        StartCoroutine(bossHpBar.SetBossHpData(boss));
    }

    public void SetHealthBar()
    {
        healthBar.fillAmount = GameManager.Instance.player.stat.hp / GameManager.Instance.player.stat.maxhp;
    }

    public void SetExpBar(float exp, float maxExp)
    {
        expBar.fillAmount = exp / maxExp;
    }

    //다른 구역으로 이동할 때 해당 구역으로 이동했다는 UI를 띄워줌
    public void EnterZoneUI(string zoneName)
    {
        enterZoneTxt.text = zoneName;
        if(showEnterZoneCoroutine != null)
        {
            StopCoroutine(showEnterZoneCoroutine);
        }
        showEnterZoneCoroutine = StartCoroutine("ShowEnterZone");
    }

    //다른 구역에 진입시 구역 이름을 화면 상단에 띄워줌
    IEnumerator ShowEnterZone()
    {
        currentTime = 0;
        Color colorTxt = enterZoneTxt.color;
        Color colorImg = enterZoneImg.color;

        while(currentTime <= onLerpTime)
        {
            currentTime += Time.deltaTime;

            colorTxt.a = Mathf.Lerp(0, 1, currentTime / onLerpTime);
            colorImg.a = Mathf.Lerp(0, 1, currentTime / onLerpTime);

            enterZoneTxt.color = colorTxt;
            enterZoneImg.color = colorImg;

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        currentTime = 0;

        while (currentTime <= offLerpTime)
        {
            currentTime += Time.deltaTime;

            colorTxt.a = Mathf.Lerp(1, 0, currentTime / onLerpTime);
            colorImg.a = Mathf.Lerp(1, 0, currentTime / onLerpTime);

            enterZoneTxt.color = colorTxt;
            enterZoneImg.color = colorImg;

            yield return new WaitForFixedUpdate();
        }

        showEnterZoneCoroutine = null;
        yield return null;
    }

    public void SetGold(int gold)
    {
        goldText.text = $"{gold} 골드";
    }
}
