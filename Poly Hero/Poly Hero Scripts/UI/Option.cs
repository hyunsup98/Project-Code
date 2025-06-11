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

    [Header("볼륨 관련 변수")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider soundSlider;

    [Header("해상도 옵션 관련 변수")]
    private List<Resolution> resolutions = new List<Resolution>();        //실행중인 기기에서 가능한 해상도를 넣을 배열
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private TMP_Dropdown dropdown_Resolution;
    [SerializeField] private Toggle toggle_FullScreen;
    public bool isFullScreen;

    [Header("그래픽 품질 관련 변수")]
    [SerializeField] private List<RenderPipelineAsset> list_renderPipeLineAssets = new List<RenderPipelineAsset>();
    [SerializeField] private TMP_Dropdown dropdown_Graphics;


    //옵션 창의 X 버튼
    public void OnCloseOption()
    {
        gameObject.SetActive(false);
    }

    //옵션 창의 적용 버튼
    public void OnAccept()
    {
        SoundManager.Instance.BGM.volume = bgmSlider.value;
        SoundManager.Instance.Sound.volume = soundSlider.value;

        isFullScreen = toggle_FullScreen.isOn;

        //dropdown_Resolution의 value값을 인덱스로 대입
        int screenIndex = dropdown_Resolution.value;

        //현재 해상도를 dropdown_Resolution에서 선택한 해상도로 변경
        if (Screen.width != resolutions[screenIndex].width || Screen.height != resolutions[screenIndex].height || Screen.fullScreen != isFullScreen)
        {
            Screen.SetResolution(resolutions[screenIndex].width, resolutions[screenIndex].height, isFullScreen);
            canvasScaler.referenceResolution.Set(resolutions[screenIndex].width, resolutions[screenIndex].height);
        }

        if(prevOption.graphicIndex != dropdown_Graphics.value)
        {
            QualitySettings.SetQualityLevel(dropdown_Graphics.value);                               //품질 퀄리티 레벨을 변경(Project Settings - Quality 에 있는 Levels를 변경하는 것)
        }

        DicOption saveOptionData = new DicOption();

        //옵션 데이터를 저장하는 함수를 불러오기
        saveOptionData.bgmVolume = bgmSlider.value;
        saveOptionData.soundVolume = soundSlider.value;
        saveOptionData.screenWidth = resolutions[screenIndex].width;
        saveOptionData.screenHeight = resolutions[screenIndex].height;
        saveOptionData.graphicIndex = dropdown_Graphics.value;
        saveOptionData.isFullScreen = isFullScreen;

        DataManager.Instance.OptionSave(saveOptionData);

        gameObject.SetActive(false);
    }

    //옵션 창의 취소 버튼
    public void OnCancel()
    {
        LoadOptionData(prevOption);
        gameObject.SetActive(false);
    }


    //저장된 옵션 데이터를 불러오기
    public void LoadOptionData(DicOption option)
    {
        SoundManager.Instance.BGM.volume = bgmSlider.value = option.bgmVolume;                          //BGM 볼륨 불러오기
        SoundManager.Instance.Sound.volume = soundSlider.value = option.soundVolume;                    //Sound 볼륨 불러오기
        dropdown_Resolution.value = option.screenIndex;                                                 //해상도 드랍다운 value 불러오기
        dropdown_Resolution.captionText.text = $"{option.screenWidth} x {option.screenHeight}";         //해상도 드랍다운 라벨을 현재 해상도로 바꿈
        dropdown_Graphics.value = option.graphicIndex;                                                  //그래픽 품질 드랍다운 value 불러오기
        toggle_FullScreen.isOn = option.isFullScreen;                                                   //전체화면 유무 bool값 가져오기
    }

    //초기 옵션 세팅
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

        ResolutionSetting();    //해상도 초기 세팅
    }

    //해상도 옵션 세팅(해상도 드랍다운 아이템 리스트를 지원 가능한 해상도들로 구성)
    private void ResolutionSetting()
    {
        for(int i = Screen.resolutions.Length - 1; i > 0; i--)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);     //현재 기기에서 지원 가능한 해상도 중에서 화면 주사율이 60 헤르츠인 해상도들만 리스트에 추가
            }
        }

        dropdown_Resolution.ClearOptions();     //해상도 dropdown에 옵션들 클리어

        dropdown_Resolution.captionText.text = $"{Screen.width} x {Screen.height}";

        //높은 해상도를 우선적으로 추가
        for (int i = 0; i < resolutions.Count; i++)
        {
            dropdown_Resolution.options.Add(new TMP_Dropdown.OptionData($"{resolutions[i].width} x {resolutions[i].height}"));

            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                dropdown_Resolution.value = i;
        }
    }

    private void OnEnable()
    {
        //옵션 창을 열 때 정보를 담아둠(취소할 때 이전 데이터로 되돌리기 위해)
        prevOption.bgmVolume = bgmSlider.value;
        prevOption.soundVolume = soundSlider.value;
        prevOption.screenWidth = Screen.width;
        prevOption.screenHeight = Screen.height;
        prevOption.screenIndex = dropdown_Resolution.value;
        prevOption.graphicIndex = dropdown_Graphics.value;
        prevOption.isFullScreen = isFullScreen;
    }
}
