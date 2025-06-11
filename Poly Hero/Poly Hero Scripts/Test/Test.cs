using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameManager.Instance.player.Exp += 50;
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameManager.Instance.player.Money += 9999999;
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            UIManager.Instance.EnterZoneUI("Å×½ºÆ®");
        }
    }
}
