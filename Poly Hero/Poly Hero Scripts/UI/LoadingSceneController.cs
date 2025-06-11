using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

using Random = UnityEngine.Random;

//LoadingSceneController는 instance가 null일 경우 LoadingUI 프리팹을 생성하기 위해 Singleton 스크립트를 사용하지 않음
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

    [Header("로딩 UI 관련 변수")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private List<string> listTips = new List<string>();
    [SerializeField] private float fakeLoadingTime = 2;      //페이크 로딩을 실행하는 시간

    [Header("다음 씬 이름을 받을 변수")]
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

        //SceneManager에서 지원하는 씬이 로드될 때 발생하는 이벤트
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
