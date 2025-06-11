using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    //���ڿ��� ���� ������ ��� ���̺�
    public ItemDropTable dropTable;

    public SpriteRenderer boxRender;

    //���ڰ� ������ ���� �̹���
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
