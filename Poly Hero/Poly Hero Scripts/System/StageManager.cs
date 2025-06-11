using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("�� ���� �� ��ܿ� ���� ���� �̸�")]
    [SerializeField] private string zoneName;

    [Header("�� ���� �� ����� �������")]
    [SerializeField] private AudioClip stageBGM;

    [Header("�� ���� �� �÷��̾ ������ Ʈ������")]
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
