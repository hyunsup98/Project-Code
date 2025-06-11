using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("씬 입장 시 상단에 보일 지역 이름")]
    [SerializeField] private string zoneName;

    [Header("씬 입장 시 재생할 배경음악")]
    [SerializeField] private AudioClip stageBGM;

    [Header("씬 입장 시 플레이어가 스폰될 트랜스폼")]
    public Transform spawnPosition;

    private void Start()
    {
        if (GameManager.Instance.gameState != GameState.Play)
        {
            GameManager.Instance.gameState = GameState.Play;
        }

        if (!UIManager.Instance.gameObject.activeSelf)
        {
            UIManager.Instance.gameObject.SetActive(true);
        }

        if (!string.IsNullOrEmpty(zoneName))
        {
            UIManager.Instance.EnterZoneUI(zoneName);
        }

        if(stageBGM != null)
        {
            SoundManager.Instance.SetBGM(stageBGM);
        }

        if(spawnPosition != null && GameManager.Instance.player != null)
        {
            GameManager.Instance.player.transform.position = spawnPosition.position;
        }

        GameManager.Instance.player.Revival();
    }
}
