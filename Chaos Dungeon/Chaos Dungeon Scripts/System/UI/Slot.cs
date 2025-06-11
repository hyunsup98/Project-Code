using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public SlotInfo slotInfo;

    private void Start()
    {
        slotInfo = transform.GetChild(0).GetComponent<SlotInfo>();
    }
}
