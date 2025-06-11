using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [Header("시작 노래")]
    [SerializeField] private AudioClip bgm;

    [Header("다음 씬")]
    [SerializeField] private string nextScene;

    [Header("옵션창")]
    [SerializeField] private GameObject optionUI;

    [SerializeField] private Button btnContinue;

    string gameDataPath;

    private void Start()
    {
        if (bgm != null)
        {
            SoundManager.Instance.SetBGM(bgm);
        }

        gameDataPath = Path.Combine(Application.dataPath + "/Save/", "gameData.json");

        if (File.Exists(gameDataPath))
        {
            btnContinue.gameObject.SetActive(true);
        }

        if(UIManager.Instance.gameObject != null)
        {
            UIManager.Instance.ClearSlotsEvent();
        }
    }

    //게임 시작 버튼 클릭 시 실행
    public void OnStartButton()
    {
        if (File.Exists(gameDataPath))
        {
            File.Delete(gameDataPath);
        }

        LoadingSceneController.Instance.LoadScene(nextScene);
    }

    //이어하기 버튼 클릭 시 실행
    public void OnContinueButton()
    {
        LoadingSceneController.Instance.LoadScene(nextScene);
    }

    //옵션 버튼 클릭 시 실행
    public void OnOptionButton()
    {
        optionUI?.SetActive(true);
    }

    //게임 종료 버튼 클릭 시 실행
    public void OnExitButton()
    {
        Application.Quit();
    }

    //옵션 창의 X 버튼
    public void OnCloseOption()
    {
        optionUI?.SetActive(false);
    }
}
