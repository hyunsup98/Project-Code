using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackGroundMove : MonoBehaviour
{
    Map map;
    Transform background;
    Vector3 center;
    // Update is called once per frame
    void Update()
    {
        if (map != GameManager.Map)
        {
            map = GameManager.Map;
            center = map.Camera_MinSize.position + (map.camera_MaxSize.position - map.Camera_MinSize.position) / 2;
            background = map.transform.Find("BackGround");
        }
        Transform player = GameManager.GetPlayer().transform;

        Vector3 local = center - player.position;
        Vector3 moveloc = center + (local * 0.1f);

        background.localPosition = Vector3.Lerp(background.localPosition, new Vector3(Mathf.Clamp(local.x * 0.02f, -0.5f, 0.5f), Mathf.Clamp(local.y * 0.002f, -0.1f, 0.1f)),GameManager.deltaTime);

        if (Vector3.Distance(transform.position, moveloc) > 5)
        {
            transform.position = moveloc;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, moveloc, GameManager.deltaTime);
        }
    }
}
