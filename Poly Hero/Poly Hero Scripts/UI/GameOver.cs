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
    
    //재시작 버튼 누를 시
    public void OnRestart()
    {
        LoadingSceneController.Instance.LoadScene("Village");
        gameObject.SetActive(false);
    }

    //메인메뉴로 버튼 누를 시
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

    //플레이어가 죽어서 켜지냐 esc를 눌러 일시정지를 하냐에 따라 명령이 변함
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
