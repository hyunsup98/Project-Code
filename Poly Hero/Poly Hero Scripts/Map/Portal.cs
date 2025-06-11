using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("포탈을 타고 이동할 다음 씬 이름")]
    [SerializeField] private string nextScene;

    [Header("플레이어가 포탈 입장 시 플레이어의 위치를 저장하기 위한 스테이지 매니저")]
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
