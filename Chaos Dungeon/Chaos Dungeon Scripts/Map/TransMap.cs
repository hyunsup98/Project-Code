using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransMap : MonoBehaviour
{
    //�̹� �ش� ��ǥ�� ���� �Ҵ�Ǿ� �ִ��� Ȯ��
    public Map map;

    public int row;
    public int col;

    public void Set(Map map, int row, int col)
    {
        this.map = map;
        this.row = row;
        this.col = col;
    }
}
