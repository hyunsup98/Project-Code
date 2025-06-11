using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Recipe
{
    public Item item;
    public int count;
}

[CreateAssetMenu]
public class ItemRecipe : ScriptableObject
{
    [Header("현재 레시피 제작을 위한 재료")]
    public List<Recipe> recipeList = new List<Recipe>();

    [Header("제작 시 얻을 수 있는 아이템")]
    public Item completeItem;
    public int count;
}
