using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

using Random = UnityEngine.Random;

//LoadingSceneController�� instance�� null�� ��� LoadingUI �������� �����ϱ� ���� Singleton ��ũ��Ʈ�� ������� ����
public class LoadingSceneController : MonoBehaviour
{
    private static LoadingSceneController instance;

    public static LoadingSceneController Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if(obj != null)
                {
                    instance = obj;
                }
            }
            return instance;
        }
    }

    [Header("�ε� UI ���� ����")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private List<string> listTips = new List<string>();
    [SerializeField] private float fakeLoadingTime = 2;      //����ũ �ε��� �����ϴ� �ð�

    [Header("���� �� �̸��� ���� ����")]
    private string loadSceneName;

    public event Action SceneMoveAction;


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SetTip();

        //SceneManager���� �����ϴ� ���� �ε�� �� �߻��ϴ� �̺�Ʈ
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        SceneMoveAction?.Invoke();
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if(op.progress < 0.7f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.7f, 1f, timer / fakeLoadingTime);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if(!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }

    private void SetTip()
    {
        if(listTips.Count > 0)
        {
            int index = Random.Range(0, listTips.Count);
            tipText.text = $"Tip! {listTips[index]}";
        }
        else
        {
            tipText.text = string.Empty;
        }
    }
}
