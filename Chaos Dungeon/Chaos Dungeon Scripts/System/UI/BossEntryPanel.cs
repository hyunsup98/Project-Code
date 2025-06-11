using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntryPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnOk()
    {
        MapManager.Instance.EntryLastMap();
    }

    public void OnNo()
    {
        MapManager.Instance.OffPanel(false);
    }
}
