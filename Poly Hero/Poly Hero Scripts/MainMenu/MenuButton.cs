using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text btnText;
    [SerializeField] AudioClip btnSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnText.fontStyle = FontStyles.Bold;
        SoundManager.Instance.SetSound(btnSound, transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnText.fontStyle -= FontStyles.Bold;
    }
}
