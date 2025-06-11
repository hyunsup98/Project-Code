using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTableUI : MonoBehaviour
{
    [Header("제작 테이블에 선택된 레시피 아이템을 보여줄 변수들")]
    [SerializeField] private Image itemImg;                 //아이템 이미지
    [SerializeField] private TMP_Text itemNameText;         //아이템 이름
    [SerializeField] private TMP_Text itemInfoText;         //아이템 설명

    [SerializeField] private RecipeSlot slot;
    [SerializeField] private Transform slotTrans;
    [SerializeField] private List<ItemRecipe> recipes = new List<ItemRecipe>();

    [SerializeField] private Transform ingreTrans;          //재료를 생성할 트랜스폼
    [SerializeField] private Ingredient ingredient;         //아이템을 만들기 위해 필요한 재료 슬롯 프리팹

    private List<Ingredient> ingredients = new List<Ingredient>();

    private Item item;
    private ItemRecipe recipe;
    private bool isCanCreate = true;

    private void Start()
    {
        if(recipes.Count > 0)
        {
            foreach(var item in recipes)
            {
                RecipeSlot rs = Instantiate(slot, slotTrans);
                rs.DataInit(item, item.completeItem.itemSprite);
            }
        }
    }

    //선택한 아이템 레시피에 맞는 데이터로 세팅하기
    public void SetData(ItemRecipe recipe)
    {
        ClearIngredient();
        isCanCreate = true;

        item = recipe.completeItem;
        this.recipe = recipe;

        Color color = itemImg.color;
        color.a = 1f;
        itemImg.color = color;

        itemImg.sprite = item.itemSprite;
        itemNameText.text = item.itemstats.name;
        itemInfoText.text = item.itemstats.description;

        foreach(var r in recipe.recipeList)
        {
            Ingredient ingre = SetIngredient();
            ingre.SetImage(r.item);
            bool isLittle = r.count > UIManager.Instance.inventory.ItemCount(r.item) ? false : true;
            ingre.textCount.text = isLittle ?
                        $"<color=green>{UIManager.Instance.inventory.ItemCount(r.item)}</color>/<color=green>{r.count}</color>" :
                        $"<color=red>{UIManager.Instance.inventory.ItemCount(r.item)}</color>/<color=green>{r.count}</color>";

            if (isCanCreate)
                isCanCreate = isLittle;
        }
    }

    //데이터 초기화
    public void DataInitialization()
    {
        Color color = itemImg.color;
        color.a = 0f;

        itemImg.color = color;
        itemNameText.text = string.Empty;
        itemInfoText.text = string.Empty;

        ClearIngredient();
    }

    //ingredient 오브젝트풀링
    public void ClearIngredient()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].gameObject.activeSelf)
            {
                ingredients[i].gameObject.SetActive(false);
            }
        }
    }

    //ingredient 오브젝트풀링
    Ingredient SetIngredient()
    {
        Ingredient ingre;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (!ingredients[i].gameObject.activeSelf)
            {
                ingre = ingredients[i];
                ingre.gameObject.SetActive(true);
                return ingre;
            }
        }
        ingre = Instantiate(ingredient, ingreTrans);
        ingredients.Add(ingre);
        return ingre;
    }

    //제작 버튼을 눌렀을 때
    public void OnCreate()
    {
        if(isCanCreate && item != null)
        {
            foreach(var re in recipe.recipeList)
            {
                UIManager.Instance.inventory.DiscountItem(re.item, re.count);
            }
            Item recipeItem = recipe.completeItem;
            recipeItem.Count = recipe.count;

            UIManager.Instance.inventory.AddItem(recipeItem);
            SetData(recipe);
        }
    }

    //나가기 버튼을 눌렀을 때
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        DataInitialization();
        GameManager.Instance.SetGameState(gameObject, false);
    }
}
