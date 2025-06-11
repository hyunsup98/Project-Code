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
        public List<int> count = new List<int>();       //��� ����
    }

    //��ȭ, ���� �������� ���� ����Ʈ
    public List<Material> ingredients = new List<Material>();
    public int gold;        //��ȭ ���
}
