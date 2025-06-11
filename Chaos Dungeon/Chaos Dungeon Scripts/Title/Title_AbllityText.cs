using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title_AbllityText : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TMP_Text text_Name;
    [SerializeField] TMP_Text text_Lore;

    public void RepInfo(AbilityInfo info)
    {
        this.img.sprite = info.img;
        text_Name.text = info.name;
        text_Lore.text = info.lore;
    }
}
