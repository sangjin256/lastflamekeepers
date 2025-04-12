using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle windowToggle;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private readonly Resolution[] supportedResolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 3840, height = 2160 },
    };

    void Start()
    {
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int current = 0;

        for (int i = 0; i < supportedResolutions.Length; i++)
        {
            var res = supportedResolutions[i];
            string option = $"{res.width} x {res.height}";
            options.Add(option);

            if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
            {
                current = i;
            }
            else if (res.width == 1920 && res.height == 1080) // 기본값
            {
                current = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = current;
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        // 창모드 토글
        windowToggle.isOn = Screen.fullScreen == false;
        windowToggle.onValueChanged.AddListener(ChangeWindowMode);

        // 사운드 슬라이더
        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        // 초기값
        bgmSlider.value = 1f;
        sfxSlider.value = 1f;
    }

    void ChangeResolution(int index)
    {
        var selectedRes = supportedResolutions[index];
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
    }

    void ChangeWindowMode(bool isWindow)
    {
        Screen.fullScreen = !isWindow;
    }

    public void OnCloseSetting(GameObject panel)
    {
        panel.SetActive(false);
    }
}
