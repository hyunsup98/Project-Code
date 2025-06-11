using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ItemDropTable itemDropTable;
    //�÷��̾ �����ȿ� ������ true
    private bool isNear;
    //�̹� ���� ���ڸ� true
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
            //eŰ ������� ui �ѱ�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && isNear && !isOpen)
        {
            isNear = false;
            animator.SetBool("isopen", false);
            //eŰ ������� ui ����
        }
    }
}
