using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : DelayBehaviour
{
    [SerializeField] private Title_Player player;
    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject ui_MainGroup;
    [SerializeField] private GameObject ui_StartGroup;

    public void Button_Start()
    {
        ui_MainGroup.SetActive(false);
        dummy.SetActive(false);
        player.spineAnimationState.SetAnimation(0, "etc", true);
        CameraMove.Instance.pos = new Vector3(3.05f, -2, -10);
        CameraMove.Instance.range = 1;
        Delay(() =>
        {
            ui_StartGroup.SetActive(true);
        }, 2);
    }
}
