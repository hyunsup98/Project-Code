using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    //상자에서 나올 아이템 드랍 테이블
    public ItemDropTable dropTable;

    public SpriteRenderer boxRender;

    //상자가 열리고 닫힐 이미지
    public Sprite openSprite;
    public Sprite closeSprite;

    bool isNearPlayer = false;
    bool isOpen = false;
    private void Update()
    {
        if (!isNearPlayer)
            return;

        if(Input.GetKeyDown(KeyCode.G) && !isOpen)
        {
            boxRender.sprite = openSprite;
            dropTable.DropItems(transform);
            UIManager.Instance.inputKeyUI.SetActive(false);
            isOpen = true;

            UtilObject.PlaySound("Chest2", transform, 0.2f, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isOpen)
        {
            UIManager.Instance.inputKeyUI.SetActive(true);
            isNearPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UIManager.Instance.inputKeyUI.SetActive(false);
        }
    }
}
