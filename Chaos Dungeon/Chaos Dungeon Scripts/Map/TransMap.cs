using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransMap : MonoBehaviour
{
    //이미 해당 좌표에 맵이 할당되어 있는지 확인
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
