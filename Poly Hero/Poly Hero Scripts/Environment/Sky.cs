using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        //skybox의 _Rotation 속성을 게임시간 * rotateSpeed의 속도로 회전시킴
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
