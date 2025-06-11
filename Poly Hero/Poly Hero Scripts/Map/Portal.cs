using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("��Ż�� Ÿ�� �̵��� ���� �� �̸�")]
    [SerializeField] private string nextScene;

    [Header("�÷��̾ ��Ż ���� �� �÷��̾��� ��ġ�� �����ϱ� ���� �������� �Ŵ���")]
    [SerializeField] private StageManager stage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            stage.spawnPosition.position = GameManager.Instance.player.transform.position;
            LoadingSceneController.Instance.LoadScene(nextScene);
        }
    }
}
