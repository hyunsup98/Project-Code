using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReinForceUI : MonoBehaviour
{
    //강화 재료 표시를 위한 변수
    [SerializeField] Transform objIngredient;
    [SerializeField] Ingredient ingredient;
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtProbability;
    [SerializeField] TMP_Text txtGold;
    [SerializeField] Slot slot;

    public Equip item;
    bool isCanForce = true;    //강화가 가능한지 정하는 bool값
    int index;

    List<Ingredient> listIngredient = new List<Ingredient>();    //꺼내쓸 리스트

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
                    bool islittle = ingre.count[index] > UIManager.Instance.inventory.ItemCount(ingre.item) ? false : true;     //요구 개수에 미치지 못했을 때
                    copyIngre.textCount.text = islittle ?
                        $"<color=green>{UIManager.Instance.inventory.ItemCount(ingre.item)}</color>/<color=green>{ingre.count[index]}</color>" :
                        $"<color=red>{UIManager.Instance.inventory.ItemCount(ingre.item)}</color>/<color=green>{ingre.count[index]}</color>";

                    if (isCanForce)
                    {
                        //재료가 하나라도 충족하지 못하면 강화가 불가능하기 때문에 isCanForce가 이미 false일 땐 실행하지 않음
                        isCanForce = islittle;
                    }
                }

                txtName.text = $"{item.itemstats.name} +{item.stats.level}";
                propability = 100 - (index * 5);
                txtProbability.text = $"성공 확률: {propability}%";
                txtGold.text = (force.gold + index * 50).ToString();
            }
        }
    }

    //ingredient 오브젝트풀링
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
        txtProbability.text = $"성공 확률:";
        txtGold.text = string.Empty;
    }

    //ingredient 오브젝트풀링
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

    //강화 실시
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
                //이미 맥스 레벨이라고 표시
                Debug.Log("강화 실패");
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

        //강화 창이 닫히는데 강화 슬롯에 템이 남아있다면 인벤토리에 자동으러 넣어줌
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
