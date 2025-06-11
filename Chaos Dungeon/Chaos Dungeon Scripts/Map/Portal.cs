using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Portal : DelayBehaviour
{
    [SerializeField] bool isOpen;
    
    public Map parentMap;
    public Transform nextPortal;
    public SpriteRenderer portalDoor;


    //문을 여닫는 애니매이션
    protected override void Updates()
    {
        float rotate = portalDoor.transform.localRotation.eulerAngles.y;
        int speed = 100;

        if (isOpen) 
            speed *= -1;
        portalDoor.sortingOrder = rotate > 45 ? 0 : 5;
        portalDoor.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(rotate + GameManager.deltaTime * speed,0,90), 0);

    }

    public void IsOpen(bool _isOpen)
    {
        if (isOpen == _isOpen)
            return;
        else
            isOpen = !isOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().isUsePortal)
        {
            //포탈 문이 열려있을때
            if (portalDoor.transform.localRotation.eulerAngles.y == 0)
            {
                Portal portal = nextPortal.GetComponent<Portal>();

                collision.gameObject.GetComponent<Player>().isUsePortal = false;
                UtilObject.PlaySound("portal", GameManager.GetPlayer().transform, 1, 1);

                //포탈을 타고 다음 맵으로 넘어가면서 다음 맵의 몬스터 웨이브를 작동
                Delay(() =>
                {
                    nextPortal.GetComponent<Portal>().parentMap.NextWave();
                },3f);
                UIManager.OnMove();
                Delay(() =>
                {
                    collision.transform.position = nextPortal.position;
                    GameManager.Map = portal.parentMap;
                    GameManager.Instance.MapCamera();
                },0.9f);
            }
        }
    }
}
