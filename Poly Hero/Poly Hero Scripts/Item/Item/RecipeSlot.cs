using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemRecipe recipe;
    [SerializeField] private Image itemImg;

    public void DataInit(ItemRecipe ir, Sprite sprite)
    {
        recipe = ir;
        itemImg.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            UIManager.Instance.createTableUI.SetData(recipe);
        }
    }
}
