using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ItemDropTable itemDropTable;
    //플레이어가 영역안에 있으면 true
    private bool isNear;
    //이미 열린 상자면 true
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
        isNear = false;
    }

    private void Update()
    {
        if(isNear && !isOpen)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("isopen", true);
                isOpen = true;
                itemDropTable.DropItems(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOpen)
        {
            isNear = true;
            //e키 누르라는 ui 켜기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && isNear && !isOpen)
        {
            isNear = false;
            animator.SetBool("isopen", false);
            //e키 누르라는 ui 끄기
        }
    }
}
