using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        //skybox�� _Rotation �Ӽ��� ���ӽð� * rotateSpeed�� �ӵ��� ȸ����Ŵ
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
