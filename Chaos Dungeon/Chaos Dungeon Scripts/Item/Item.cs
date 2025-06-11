using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer render;

    //생성 되자마자 바로 플레이어에게 빨려 들어가지 않게 하기 위한 bool값, 방금 막 드랍된건지 체크
    bool isDrop = true;

    //아이템 개수
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

    //아이템의 스탯(데미지, 아이템타입, 이미지, 아이템 설명)
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
