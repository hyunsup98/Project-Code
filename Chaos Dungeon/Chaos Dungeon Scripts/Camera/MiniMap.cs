using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Camera myCamera;
    Map map;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(map != GameManager.Map)
        {
            map = GameManager.Map;
            SizeRep();
        }
    }
    void SizeRep()
    {
        Bounds bounds = map.transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds;
        transform.position = bounds.center + new Vector3(0, 0, -10);
        myCamera.fieldOfView = 60 + Mathf.Max(bounds.extents.x, bounds.extents.y) * 4;
    }
}
