using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    DicOption prevOption = new DicOption();
    DicOption option = new DicOption();

    [Header("���� ���� ����")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider soundSlider;

    [Header("�ػ� �ɼ� ���� ����")]
    private List<Resolution> resolutions = new List<Resolution>();        //�������� ��⿡�� ������ �ػ󵵸� ���� �迭
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private TMP_Dropdown dropdown_Resolution;
    [SerializeField] private Toggle toggle_FullScreen;
    public bool isFullScreen;

    [Header("�׷��� ǰ�� ���� ����")]
    [SerializeField] private List<RenderPipelineAsset> list_renderPipeLineAssets = new List<RenderPipelineAsset>();
    [SerializeField] private TMP_Dropdown dropdown_Graphics;


    //�ɼ� â�� X ��ư
    public void OnCloseOption()
    {
        gameObject.SetActive(false);
    }

    //�ɼ� â�� ���� ��ư
    public void OnAccept()
    {
        SoundManager.Instance.BGM.volume = bgmSlider.value;
        SoundManager.Instance.Sound.volume = soundSlider.value;

        isFullScreen = toggle_FullScreen.isOn;

        //dropdown_Resolution�� value���� �ε����� ����
        int screenIndex = dropdown_Resolution.value;

        //���� �ػ󵵸� dropdown_Resolution���� ������ �ػ󵵷� ����
        if (Screen.width != resolutions[screenIndex].width || Screen.height != resolutions[screenIndex].height || Screen.fullScreen != isFullScreen)
        {
            Screen.SetResolution(resolutions[screenIndex].width, resolutions[screenIndex].height, isFullScreen);
            canvasScaler.referenceResolution.Set(resolutions[screenIndex].width, resolutions[screenIndex].height);
        }

        if(prevOption.graphicIndex != dropdown_Graphics.value)
        {
            QualitySettings.SetQualityLevel(dropdown_Graphics.value);                               //ǰ�� ����Ƽ ������ ����(Project Settings - Quality �� �ִ� Levels�� �����ϴ� ��)
        }

        DicOption saveOptionData = new DicOption();

        //�ɼ� �����͸� �����ϴ� �Լ��� �ҷ�����
        saveOptionData.bgmVolume = bgmSlider.value;
        saveOptionData.soundVolume = soundSlider.value;
        saveOptionData.screenWidth = resolutions[screenIndex].width;
        saveOptionData.screenHeight = resolutions[screenIndex].height;
        saveOptionData.graphicIndex = dropdown_Graphics.value;
        saveOptionData.isFullScreen = isFullScreen;

        DataManager.Instance.OptionSave(saveOptionData);

        gameObject.SetActive(false);
    }

    //�ɼ� â�� ��� ��ư
    public void OnCancel()
    {
        LoadOptionData(prevOption);
        gameObject.SetActive(false);
    }


    //����� �ɼ� �����͸� �ҷ�����
    public void LoadOptionData(DicOption option)
    {
        SoundManager.Instance.BGM.volume = bgmSlider.value = option.bgmVolume;                          //BGM ���� �ҷ�����
        SoundManager.Instance.Sound.volume = soundSlider.value = option.soundVolume;                    //Sound ���� �ҷ�����
        dropdown_Resolution.value = option.screenIndex;                                                 //�ػ� ����ٿ� value �ҷ�����
        dropdown_Resolution.captionText.text = $"{option.screenWidth} x {option.screenHeight}";         //�ػ� ����ٿ� ���� ���� �ػ󵵷� �ٲ�
        dropdown_Graphics.value = option.graphicIndex;                                                  //�׷��� ǰ�� ����ٿ� value �ҷ�����
        toggle_FullScreen.isOn = option.isFullScreen;                                                   //��üȭ�� ���� bool�� ��������
    }

    //�ʱ� �ɼ� ����
    public void OptionSetting()
    {
        option = DataManager.Instance.OptionLoad();
        if (option != null)
        {
            LoadOptionData(option);
            Screen.SetResolution(option.screenWidth, option.screenHeight, option.isFullScreen);
            QualitySettings.SetQualityLevel(option.graphicIndex);                               
            QualitySettings.renderPipeline = list_renderPipeLineAssets[option.graphicIndex];
        }

        ResolutionSetting();    //�ػ� �ʱ� ����
    }

    //�ػ� �ɼ� ����(�ػ� ����ٿ� ������ ����Ʈ�� ���� ������ �ػ󵵵�� ����)
    private void ResolutionSetting()
    {
        for(int i = Screen.resolutions.Length - 1; i > 0; i--)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);     //���� ��⿡�� ���� ������ �ػ� �߿��� ȭ�� �ֻ����� 60 �츣���� �ػ󵵵鸸 ����Ʈ�� �߰�
            }
        }

        dropdown_Resolution.ClearOptions();     //�ػ� dropdown�� �ɼǵ� Ŭ����

        dropdown_Resolution.captionText.text = $"{Screen.width} x {Screen.height}";

        //���� �ػ󵵸� �켱������ �߰�
        for (int i = 0; i < resolutions.Count; i++)
        {
            dropdown_Resolution.options.Add(new TMP_Dropdown.OptionData($"{resolutions[i].width} x {resolutions[i].height}"));

            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                dropdown_Resolution.value = i;
        }
    }

    private void OnEnable()
    {
        //�ɼ� â�� �� �� ������ ��Ƶ�(����� �� ���� �����ͷ� �ǵ����� ����)
        prevOption.bgmVolume = bgmSlider.value;
        prevOption.soundVolume = soundSlider.value;
        prevOption.screenWidth = Screen.width;
        prevOption.screenHeight = Screen.height;
        prevOption.screenIndex = dropdown_Resolution.value;
        prevOption.graphicIndex = dropdown_Graphics.value;
        prevOption.isFullScreen = isFullScreen;
    }
}
