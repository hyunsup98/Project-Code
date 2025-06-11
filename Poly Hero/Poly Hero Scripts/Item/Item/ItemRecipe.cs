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
    [Header("���� ������ ������ ���� ���")]
    public List<Recipe> recipeList = new List<Recipe>();

    [Header("���� �� ���� �� �ִ� ������")]
    public Item completeItem;
    public int count;
}
