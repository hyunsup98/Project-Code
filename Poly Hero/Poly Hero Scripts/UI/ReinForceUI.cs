using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReinForceUI : MonoBehaviour
{
    //��ȭ ��� ǥ�ø� ���� ����
    [SerializeField] Transform objIngredient;
    [SerializeField] Ingredient ingredient;
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtProbability;
    [SerializeField] TMP_Text txtGold;
    [SerializeField] Slot slot;

    public Equip item;
    bool isCanForce = true;    //��ȭ�� �������� ���ϴ� bool��
    int index;

    List<Ingredient> listIngredient = new List<Ingredient>();    //������ ����Ʈ

    int propability = 0;

    [SerializeField] private GameObject dialogUI;

    public void ShowIngredient(Equip item)
    {
        ClearIngredient();

        if (item != null)
        {
            this.item = item;
            ForceTable force = item.forceTable;
            index = item.stats.level - 1;
            isCanForce = true;

            if (item.GetComponent<IForce>() != null && item.stats.level <= item.stats.maxLevel)
            {
                foreach (var ingre in force.ingredients)
                {
                    Ingredient copyIngre = SetIngredient();
                    copyIngre.SetImage(ingre.item);
                    bool islittle = ingre.count[index] > UIManager.Instance.inventory.ItemCount(ingre.item) ? false : true;     //�䱸 ������ ��ġ�� ������ ��
                    copyIngre.textCount.text = islittle ?
                        $"<color=green>{UIManager.Instance.inventory.ItemCount(ingre.item)}</color>/<color=green>{ingre.count[index]}</color>" :
                        $"<color=red>{UIManager.Instance.inventory.ItemCount(ingre.item)}</color>/<color=green>{ingre.count[index]}</color>";

                    if (isCanForce)
                    {
                        //��ᰡ �ϳ��� �������� ���ϸ� ��ȭ�� �Ұ����ϱ� ������ isCanForce�� �̹� false�� �� �������� ����
                        isCanForce = islittle;
                    }
                }

                txtName.text = $"{item.itemstats.name} +{item.stats.level}";
                propability = 100 - (index * 5);
                txtProbability.text = $"���� Ȯ��: {propability}%";
                txtGold.text = (force.gold + index * 50).ToString();
            }
        }
    }

    //ingredient ������ƮǮ��
    public void ClearIngredient()
    {
        item = null;
        for(int i = 0; i < listIngredient.Count; i++)
        {
            if (listIngredient[i].gameObject.activeSelf)
            {
                listIngredient[i].gameObject.SetActive(false);
            }
        }
        txtProbability.text = $"���� Ȯ��:";
        txtGold.text = string.Empty;
    }

    //ingredient ������ƮǮ��
    Ingredient SetIngredient()
    {
        Ingredient ingre;

        for(int i = 0; i < listIngredient.Count; i++)
        {
            if (!listIngredient[i].gameObject.activeSelf)
            {
                ingre = listIngredient[i];
                ingre.gameObject.SetActive(true);
                return ingre;
            }
        }
        ingre = Instantiate(ingredient, objIngredient);
        listIngredient.Add(ingre);
        return ingre;
    }

    //��ȭ �ǽ�
    public void OnForceItem()
    {
        if(item != null)
        {
            if (item.stats.level < item.stats.maxLevel && isCanForce)
            {
                foreach (var i in item.forceTable.ingredients)
                {
                    UIManager.Instance.inventory.DiscountItem(i.item, i.count[index]);
                }

                int randProp = Random.Range(0, 100);
                if (randProp < propability)
                {
                    item.Force();
                    ShowIngredient(item);
                }
            }
            else
            {
                //�̹� �ƽ� �����̶�� ǥ��
                Debug.Log("��ȭ ����");
            }
        }
    }

    public void OnExitBtn()
    {
        if(dialogUI != null && dialogUI.activeSelf)
            dialogUI.SetActive(false);

        gameObject.SetActive(false);
    }



    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
        transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        GameManager.Instance.SetGameState(gameObject, false);

        //��ȭ â�� �����µ� ��ȭ ���Կ� ���� �����ִٸ� �κ��丮�� �ڵ����� �־���
        if (item != null)
        {
            UIManager.Instance.inventory.MoveItem(item);
            item = null;
            slot.ClearSlot();
        }
        
        if(UIManager.Instance.dialogUI.gameObject.activeSelf)
        {
            UIManager.Instance.dialogUI.gameObject.SetActive(false);
        }
    }
}
