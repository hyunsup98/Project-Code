using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : DelayBehaviour
{
    [SerializeField] GameObject uis;
    [SerializeField] Inventory invs;

    private void OnEnable()
    {
        for (int i = 0; i < uis.transform.childCount; i++)
        {
            Transform child = uis.transform.GetChild(i);
            if (child != this.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(true);
        Camera.main.GetComponent<BloadShader>().rgbSplit = 3;
        Delay(() =>
        {
            SceneManager.LoadScene(0);
            Delay(() =>
            {
                for (int i = 0; i < ItemManager.Instance.transform.childCount; i++)
                {
                    ItemManager.Instance.transform.GetChild(i).gameObject.SetActive(false);
                }
                invs.Clear();
                GameManager.SetUI(false);
            }, 0.5f);
        }, 5);
    }
}
