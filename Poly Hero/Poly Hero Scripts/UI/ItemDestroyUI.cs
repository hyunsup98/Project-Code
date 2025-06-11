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

    //������ �ı� Ȯ�ι�ư Ŭ�� �� ����
    public void OnDestroyOK()
    {
        if (count > destroySlot.item.Count)
            count = destroySlot.item.Count;

        destroySlot.item.Count -= count;
        destroySlot.CheckItemCount();
        gameObject.SetActive(false);
    }

    //������ �ı� ��ҹ�ư Ŭ�� �� ����
    public void OnDestroyCancle()
    {
        gameObject.SetActive(false);
    }

    //�ı��� ������ ���� �Է� �� �ٸ� ������ Ŭ���� ��� ������ �Լ�
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
