using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class DicSkill
{
    //스킬 관련 데이터들
    public int key;
    public Skill value;
}

[System.Serializable]
public class DicItem
{
    //아이템 관련 데이터들
    public int key;         //슬롯 인덱스
    public int id;          //아이템 아이디
    public int count;       //아이템 개수
}

[System.Serializable]
public class DicEquip
{
    //장착 가능한 아이템 관련 데이터들
    public int key;         //슬롯 인덱스
    public int id;          //착용 아이템의 아이디
    public int level;       //착용 아이템의 강화 수치
}

[System.Serializable]
public class DicOption
{
    //옵션 관련 데이터들
    public float bgmVolume;     //브금 소리 크기
    public float soundVolume;   //사운드 소리 크기
    public int screenWidth;     //스크린 넓이
    public int screenHeight;    //스크린 높이
    public int screenIndex;     //해상도 드롭다운 인덱스
    public int graphicIndex;    //그래픽 품질 인덱스
    public bool isFullScreen;   //전체화면인지 체크(true면 전체화면)
}

[System.Serializable]
public class SaveData
{
    public List<DicItem> inventorySlots = new List<DicItem>();          //인벤토리 아이템 슬롯
    public List<DicEquip> inventorySlotEquips = new List<DicEquip>();   //인벤토리 아이템(착용 종류) 슬롯
    public List<DicItem> quickSlots = new List<DicItem>();              //단축키 슬롯
    public List<DicEquip> quickSlotEquips = new List<DicEquip>();       //단축키 아이템(착용 종류) 슬롯
    public List<DicSkill> skillSlots = new List<DicSkill>();            //스킬 슬롯

    public int gold;
    public int level;
    public float exp;

    public Vector3 playerPos;
}

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<Item> items = new List<Item>();

    string originPath;      //모든 데이터가 저장될 폴더 경로
    string gameDataPath;    //게임 데이터가 저장될 파일 경로
    string optionDataPath;  //옵션 데이터가 저장될 파일 경로

    private void Start()
    {
        originPath = Path.Combine(Application.dataPath + "/Save/");
        gameDataPath = Path.Combine(originPath, "gameData.json");
        optionDataPath = Path.Combine(originPath, "optionData.json");
    }

    //데이터를 json 파일 형식으로 저장하기
    public void DataSave()
    {
        SaveData saveData = new SaveData();

        //아이템이 있는 인벤토리 슬롯의 아이템은 딕셔너리에 저장
        for (int i = 0; i < UIManager.Instance.inventory.slots.Count; i++)
        {
            SaveItem(UIManager.Instance.inventory.slots[i].item, i, ref saveData.inventorySlotEquips, ref saveData.inventorySlots);
        }

        //아이템이 있는 퀵슬롯의 아이템은 딕셔너리에 저장
        for (int i = 0; i < GameManager.Instance.player.quickSlots.Count; i++)
        {
            SaveItem(GameManager.Instance.player.quickSlots[i].item, i, ref saveData.quickSlotEquips, ref saveData.quickSlots);
        }

        //스킬이 있는 퀵슬롯의 스킬은 딕셔너리에 저장
        for (int i = 0; i < GameManager.Instance.player.skillQuickSlots.Count; i++)
        {
            if (GameManager.Instance.player.skillQuickSlots[i].skill != null)
            {
                DicSkill dic = new DicSkill();
                dic.key = i;
                dic.value = GameManager.Instance.player.skillQuickSlots[i].skill;
                saveData.skillSlots.Add(dic);
            }
        }

        saveData.gold = GameManager.Instance.player.Money;
        saveData.level = GameManager.Instance.player.Level;
        saveData.exp = GameManager.Instance.player.Exp;
        saveData.playerPos = GameManager.Instance.player.transform.position;

        UtfFromString(saveData, gameDataPath);
    }

    //json 파일 형식 데이터를 로드하기
    public void DataLoad()
    {
        SaveData saveData = new SaveData();

        if (File.Exists(gameDataPath))
        {
            StringFromUtf(ref saveData, gameDataPath);

            if (saveData != null)
            {
                //인벤토리 아이템 슬롯 데이터가 있으면 인벤토리에 아이템 넣어주기
                if(saveData.inventorySlots.Count > 0 || saveData.inventorySlotEquips.Count > 0 && items.Count > 0)
                {
                    LoadItem(UIManager.Instance.inventory.slots, saveData.inventorySlotEquips, saveData.inventorySlots);
                }

                //단축키 슬롯에 들어간 아이템이 있을 경우 슬롯에 아이템 넣어주기
                if(saveData.quickSlots.Count > 0 || saveData.quickSlotEquips.Count > 0 && items.Count > 0)
                {
                    LoadItem(GameManager.Instance.player.quickSlots, saveData.quickSlotEquips, saveData.quickSlots);
                }

                //스킬 슬롯에 들어간 스킬이 있을 경우 슬롯에 스킬 넣어주기
                if(saveData.skillSlots.Count > 0)
                {
                    foreach(var dic in saveData.skillSlots)
                    {
                        GameManager.Instance.player.skillQuickSlots[dic.key].SetSkill(dic.value);
                    }
                }

                GameManager.Instance.player.Money = saveData.gold;
                GameManager.Instance.player.Level = saveData.level;
                GameManager.Instance.player.Exp = saveData.exp;
                GameManager.Instance.player.transform.position = saveData.playerPos;
            }
        }
    }

    public void OptionSave(DicOption optionData)
    {
        UtfFromString(optionData, optionDataPath);
    }

    public DicOption OptionLoad()
    {
        DicOption optionData = new DicOption();

        if(File.Exists(optionDataPath))
        {
            StringFromUtf(ref optionData, optionDataPath);
            return optionData;
        }

        return null;
    }


    //string을 utf로 인코딩
    private void UtfFromString <T>(T Data, string path)
    {
        string[] pathSplit = path.Split("/");
        string curPath = string.Empty;

        //path 경로에 폴더/파일이 있는지 체크 후 없다면 직접 폴더/파일을 생성
        for(int i = 0; i < pathSplit.Length; i++)
        {
            curPath += pathSplit[i];

            //폴더가 없다면 생성, 마지막 주소를 제외하는 이유는 마지막 주소는 파일명이 들어있기 때문(폴더가 아니라서)
            if(i < pathSplit.Length - 1)
            {
                if(!Directory.Exists(curPath))
                {
                    Directory.CreateDirectory(curPath);
                }
                curPath += "/";
            }
            else
            {
                if(!File.Exists(curPath))
                {
                    FileStream file = File.Create(curPath);

                    //파일 생성 직후 파일이 열려있는 상태이기 때문에 수정하기 위해서 먼저 닫아줘야함
                    file.Close();
                }
            }
        }

        string saveJson = JsonUtility.ToJson(Data, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(saveJson);
        string encodeJson = System.Convert.ToBase64String(bytes);
        File.WriteAllText(path, encodeJson);
    }

    //utf를 string으로 디코딩
    private void StringFromUtf <T>(ref T Data, string path)
    {
        string loadJson = File.ReadAllText(path);
        byte[] bytes = System.Convert.FromBase64String(loadJson);
        string decodeJson = System.Text.Encoding.UTF8.GetString(bytes);
        Data = JsonUtility.FromJson<T>(decodeJson);
    }


    //아이템 분류에 따라 저장하는 방식이 다름
    private void SaveItem(Item item, int index, ref List<DicEquip> listEquips, ref List<DicItem> listItems)
    {
        if (item != null)
        {
            if (item.GetComponent<Equip>() != null)
            {
                DicEquip dic = new DicEquip();
                dic.key = index;
                dic.id = item.itemstats.id;
                dic.level = item.GetComponent<Equip>().stats.level;
                listEquips.Add(dic);
            }
            else
            {
                DicItem dic = new DicItem();
                dic.key = index;
                dic.id = item.itemstats.id;
                dic.count = item.Count;
                listItems.Add(dic);
            }
        }
    }

    //아이템 분류에 따라 불러오는 방식이 다름
    private void LoadItem(List<Slot> slots, List<DicEquip> equipList, List<DicItem> itemList)
    {
        foreach (var dic in itemList)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (dic.id == items[i].itemstats.id)
                {
                    Item item = ItemManager.Instance.Get(items[i], transform);
                    item.Count = dic.count;
                    slots[dic.key].AddSlotData(item);
                }
            }
        }

        foreach (var dic in equipList)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (dic.id == items[i].itemstats.id)
                {
                    Item item = ItemManager.Instance.Get(items[i], transform);
                    item.GetComponent<Equip>().stats.level = dic.level;
                    slots[dic.key].AddSlotData(item);
                }
            }
        }
    }
}
