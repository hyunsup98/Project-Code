using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingSkill_Collider : MonoBehaviour
{
    [SerializeField] Image guage;
    [SerializeField] GameObject bar;
    [SerializeField] RectTransform rectTrans;

    //차징스킬 성공 판정 여부
    public bool isSuccess = false;

    private void Update()
    {
        float posX = bar.GetComponent<RectTransform>().rect.width * guage.fillAmount;
        rectTrans.localPosition = new Vector3(posX, 0, 0);
    }

    public bool CheckSuccess()
    {
        guage.fillAmount = 0;
        rectTrans.localPosition = Vector3.zero;

        return isSuccess;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SuccessZone")
            isSuccess = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SuccessZone")
            isSuccess = false;
    }
}
