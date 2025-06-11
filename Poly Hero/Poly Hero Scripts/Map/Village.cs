using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Village : MonoBehaviour
{
    private void Start()
    {
        DataManager.Instance.DataLoad();

        if(UIManager.Instance.bossHpBar.gameObject.activeSelf)
        {
            UIManager.Instance.bossHpBar.gameObject.SetActive(false);
        }
    }
}
