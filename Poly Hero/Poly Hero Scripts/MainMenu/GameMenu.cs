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
    [Header("���� �뷡")]
    [SerializeField] private AudioClip bgm;

    [Header("���� ��")]
    [SerializeField] private string nextScene;

    [Header("�ɼ�â")]
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

    //���� ���� ��ư Ŭ�� �� ����
    public void OnStartButton()
    {
        if (File.Exists(gameDataPath))
        {
            File.Delete(gameDataPath);
        }

        LoadingSceneController.Instance.LoadScene(nextScene);
    }

    //�̾��ϱ� ��ư Ŭ�� �� ����
    public void OnContinueButton()
    {
        LoadingSceneController.Instance.LoadScene(nextScene);
    }

    //�ɼ� ��ư Ŭ�� �� ����
    public void OnOptionButton()
    {
        optionUI?.SetActive(true);
    }

    //���� ���� ��ư Ŭ�� �� ����
    public void OnExitButton()
    {
        Application.Quit();
    }

    //�ɼ� â�� X ��ư
    public void OnCloseOption()
    {
        optionUI?.SetActive(false);
    }
}
