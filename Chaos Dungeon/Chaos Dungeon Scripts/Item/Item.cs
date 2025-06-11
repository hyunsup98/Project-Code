using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer render;

    //���� ���ڸ��� �ٷ� �÷��̾�� ���� ���� �ʰ� �ϱ� ���� bool��, ��� �� ����Ȱ��� üũ
    bool isDrop = true;

    //������ ����
    private int count = 1;
    public int Count
    {
        get { return count; }
        set
        {
            if (value > itemstat.maxStack)
                value = itemstat.maxStack;
            count = value;
        }
    }

    //�������� ����(������, ������Ÿ��, �̹���, ������ ����)
    public ItemStats itemstat = new ItemStats();

    public virtual void AttackEvent(EntityDamageEvent evt)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        if (collision.CompareTag("Player") && !isDrop)
        {
            UIManager.Instance.obj_Inventory.GetComponent<Inventory>().SetInven(this);
            UIManager.Instance.SetWeaponUI();
            transform.position = Vector2.zero;
        }
    }

    public void BoolChange()
    {
        Invoke("BoolState", 1.5f);
    }

    void BoolState()
    {
        isDrop = false;
    }

    private void OnDisable()
    {
        count = 1;
        rb.gravityScale = 1;
        isDrop = true;
    }
}
