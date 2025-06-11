using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject btnReStart;
    [SerializeField] private TMP_Text text;

    [SerializeField] private GameObject optionUI;
    
    //����� ��ư ���� ��
    public void OnRestart()
    {
        LoadingSceneController.Instance.LoadScene("Village");
        gameObject.SetActive(false);
    }

    //���θ޴��� ��ư ���� ��
    public void OnMainMenu()
    {
        DataManager.Instance.DataSave();
        UIManager.Instance.gameObject.SetActive(false);
        GameManager.Instance.gameState = GameState.Stop;
        LoadingSceneController.Instance.LoadScene("MainMenu");
        gameObject.SetActive(false);
    }

    public void OnOption()
    {
        optionUI.SetActive(true);
        gameObject.SetActive(false);
    }

    //�÷��̾ �׾ ������ esc�� ���� �Ͻ������� �ϳĿ� ���� ����� ����
    public void Enable(bool isOn, string txt)
    {
        btnReStart?.SetActive(isOn);
        text.text = txt;
        gameObject.SetActive(true);
    }


    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        GameManager.Instance.SetGameState(gameObject, false);
    }
}
