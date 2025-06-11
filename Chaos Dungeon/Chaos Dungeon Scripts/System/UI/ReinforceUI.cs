using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceUI : MonoBehaviour
{
    //강화할 무기
    public SlotInfo weapon;

    public GameObject maxGradeText;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }

    public void OnReinforce()
    {
        if(weapon.weapItem != null)
        {
            ItemGrade grade = weapon.item.itemstat.grade;

            switch (grade)
            {
                case ItemGrade.COMMON:
                    ForceItem(GameManager.Instance.normalItems, 1000);
                    break;
                case ItemGrade.NORMAL:
                    ForceItem(GameManager.Instance.rareItems, 1001);
                    break;
                case ItemGrade.RARE:
                    ForceItem(GameManager.Instance.epicItems, 1002);
                    break;
                case ItemGrade.EPIC:
                    ForceItem(GameManager.Instance.legendItems, 1003);
                    break;
                case ItemGrade.LEGEND:
                    StartCoroutine(ShowMaxUI("이미 최대 등급의 장비입니다."));
                    break;
            }

            weapon.GetComponent<Image>().sprite = weapon.item.render.sprite;
        }
    }

    void ForceItem(List<Item> listItem, int id)
    {
        SlotInfo s = UIManager.Instance.obj_Inventory.GetComponent<Inventory>().CheckItem(id);

        if(s != null)
        {
            s.TakeItem();
            for(int i = 0; i<listItem.Count;i++)
            {
                if (listItem[i].itemstat.type == weapon.item.itemstat.type)
                {
                    weapon.item = listItem[i];
                }
            }
        }
        else
        {
            StartCoroutine(ShowMaxUI("강화에 필요한 재료를 소지하고 있지 않습니다."));
        }
    }

    IEnumerator ShowMaxUI(string str)
    {
        maxGradeText.GetComponent<TMP_Text>().text = str;
        maxGradeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        maxGradeText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(weapon.item != null)
        {
            UIManager.Instance.obj_Inventory.GetComponent<Inventory>().SetInven(weapon.item);
            UIManager.Instance.SetWeaponUI();
            weapon.item = null;
        }
    }
}
