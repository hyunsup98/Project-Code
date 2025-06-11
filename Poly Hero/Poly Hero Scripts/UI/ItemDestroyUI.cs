using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDestroyUI : MonoBehaviour
{
    [SerializeField] TMP_InputField inputcount;
    Slot destroySlot;

    int count;

    public void SetDestroyItem(Slot slot)
    {
        gameObject.SetActive(true);
        destroySlot = slot;
    }

    //아이템 파괴 확인버튼 클릭 시 실행
    public void OnDestroyOK()
    {
        if (count > destroySlot.item.Count)
            count = destroySlot.item.Count;

        destroySlot.item.Count -= count;
        destroySlot.CheckItemCount();
        gameObject.SetActive(false);
    }

    //아이템 파괴 취소버튼 클릭 시 실행
    public void OnDestroyCancle()
    {
        gameObject.SetActive(false);
    }

    //파괴할 아이템 개수 입력 후 다른 공간을 클릭할 경우 실행할 함수
    public void OnValueChangeCount()
    {
        string text = inputcount.text;

        if (int.TryParse(text, out count))
        {
            count = int.Parse(text);
        }

        if(count > destroySlot.item.Count)
        {
            inputcount.text = destroySlot.item.Count.ToString();
        }
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        destroySlot = null;
        count = 0;
        inputcount.text = string.Empty;
    }
}
