using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class DicSkill
{
    //��ų ���� �����͵�
    public int key;
    public Skill value;
}

[System.Serializable]
public class DicItem
{
    //������ ���� �����͵�
    public int key;         //���� �ε���
    public int id;          //������ ���̵�
    public int count;       //������ ����
}

[System.Serializable]
public class DicEquip
{
    //���� ������ ������ ���� �����͵�
    public int key;         //���� �ε���
    public int id;          //���� �������� ���̵�
    public int level;       //���� �������� ��ȭ ��ġ
}

[System.Serializable]
public class DicOption
{
    //�ɼ� ���� �����͵�
    public float bgmVolume;     //��� �Ҹ� ũ��
    public float soundVolume;   //���� �Ҹ� ũ��
    public int screenWidth;     //��ũ�� ����
    public int screenHeight;    //��ũ�� ����
    public int screenIndex;     //�ػ� ��Ӵٿ� �ε���
    public int graphicIndex;    //�׷��� ǰ�� �ε���
    public bool isFullScreen;   //��üȭ������ üũ(true�� ��üȭ��)
}

[System.Serializable]
public class SaveData
{
    public List<DicItem> inventorySlots = new List<DicItem>();          //�κ��丮 ������ ����
    public List<DicEquip> inventorySlotEquips = new List<DicEquip>();   //�κ��丮 ������(���� ����) ����
    public List<DicItem> quickSlots = new List<DicItem>();              //����Ű ����
    public List<DicEquip> quickSlotEquips = new List<DicEquip>();       //����Ű ������(���� ����) ����
    public List<DicSkill> skillSlots = new List<DicSkill>();            //��ų ����

    public int gold;
    public int level;
    public float exp;

    public Vector3 playerPos;
}

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<Item> items = new List<Item>();

    string originPath;      //��� �����Ͱ� ����� ���� ���
    string gameDataPath;    //���� �����Ͱ� ����� ���� ���
    string optionDataPath;  //�ɼ� �����Ͱ� ����� ���� ���

    private void Start()
    {
        originPath = Path.Combine(Application.dataPath + "/Save/");
        gameDataPath = Path.Combine(originPath, "gameData.json");
        optionDataPath = Path.Combine(originPath, "optionData.json");
    }

    //�����͸� json ���� �������� �����ϱ�
    public void DataSave()
    {
        SaveData saveData = new SaveData();

        //�������� �ִ� �κ��丮 ������ �������� ��ųʸ��� ����
        for (int i = 0; i < UIManager.Instance.inventory.slots.Count; i++)
        {
            SaveItem(UIManager.Instance.inventory.slots[i].item, i, ref saveData.inventorySlotEquips, ref saveData.inventorySlots);
        }

        //�������� �ִ� �������� �������� ��ųʸ��� ����
        for (int i = 0; i < GameManager.Instance.player.quickSlots.Count; i++)
        {
            SaveItem(GameManager.Instance.player.quickSlots[i].item, i, ref saveData.quickSlotEquips, ref saveData.quickSlots);
        }

        //��ų�� �ִ� �������� ��ų�� ��ųʸ��� ����
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

    //json ���� ���� �����͸� �ε��ϱ�
    public void DataLoad()
    {
        SaveData saveData = new SaveData();

        if (File.Exists(gameDataPath))
        {
            StringFromUtf(ref saveData, gameDataPath);

            if (saveData != null)
            {
                //�κ��丮 ������ ���� �����Ͱ� ������ �κ��丮�� ������ �־��ֱ�
                if(saveData.inventorySlots.Count > 0 || saveData.inventorySlotEquips.Count > 0 && items.Count > 0)
                {
                    LoadItem(UIManager.Instance.inventory.slots, saveData.inventorySlotEquips, saveData.inventorySlots);
                }

                //����Ű ���Կ� �� �������� ���� ��� ���Կ� ������ �־��ֱ�
                if(saveData.quickSlots.Count > 0 || saveData.quickSlotEquips.Count > 0 && items.Count > 0)
                {
                    LoadItem(GameManager.Instance.player.quickSlots, saveData.quickSlotEquips, saveData.quickSlots);
                }

                //��ų ���Կ� �� ��ų�� ���� ��� ���Կ� ��ų �־��ֱ�
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


    //string�� utf�� ���ڵ�
    private void UtfFromString <T>(T Data, string path)
    {
        string[] pathSplit = path.Split("/");
        string curPath = string.Empty;

        //path ��ο� ����/������ �ִ��� üũ �� ���ٸ� ���� ����/������ ����
        for(int i = 0; i < pathSplit.Length; i++)
        {
            curPath += pathSplit[i];

            //������ ���ٸ� ����, ������ �ּҸ� �����ϴ� ������ ������ �ּҴ� ���ϸ��� ����ֱ� ����(������ �ƴ϶�)
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

                    //���� ���� ���� ������ �����ִ� �����̱� ������ �����ϱ� ���ؼ� ���� �ݾ������
                    file.Close();
                }
            }
        }

        string saveJson = JsonUtility.ToJson(Data, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(saveJson);
        string encodeJson = System.Convert.ToBase64String(bytes);
        File.WriteAllText(path, encodeJson);
    }

    //utf�� string���� ���ڵ�
    private void StringFromUtf <T>(ref T Data, string path)
    {
        string loadJson = File.ReadAllText(path);
        byte[] bytes = System.Convert.FromBase64String(loadJson);
        string decodeJson = System.Text.Encoding.UTF8.GetString(bytes);
        Data = JsonUtility.FromJson<T>(decodeJson);
    }


    //������ �з��� ���� �����ϴ� ����� �ٸ�
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

    //������ �з��� ���� �ҷ����� ����� �ٸ�
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
