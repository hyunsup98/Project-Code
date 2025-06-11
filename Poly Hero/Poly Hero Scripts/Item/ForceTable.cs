using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ForceTable : ScriptableObject
{
    [System.Serializable]
    public class Material
    {
        public Item item;
        public List<int> count = new List<int>();       //재료 개수
    }

    //강화, 제작 아이템을 넣을 리스트
    public List<Material> ingredients = new List<Material>();
    public int gold;        //강화 비용
}
