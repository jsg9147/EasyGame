using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;
using TMPro;


public class GameOption : MonoBehaviour
{
    [SerializeField] GameObject optionWindow;
    [SerializeField] GameObject keySettingWindow;
    [SerializeField] GameObject menuWindow;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    List<Resolution> resolutions = new List<Resolution>();

    [SerializeField] Slider bgmSoundSlider;
    [SerializeField] Slider effectSoundSlider;

    float bgmVolume;
    float effectVolume;

    int resolution_Width, resolution_Height;

    private void Awake()
    {
        KeyAssign();
    }

    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        optionWindow.SetActive(false);
        Time.timeScale = menuWindow.activeSelf ? 0 : 1;
        resolution_Width = ES3.Load("Resolution_Width", 1600);
        resolution_Height = ES3.Load("Resolution_Height", 900);
        SoundInitialize();
        ResolutionInitialize();
    }

    void KeyAssign()
    {
        if (KeySetting.keys.Count == 0)
        {
            KeySetting.keys.Add(KeyAction.UP, (KeyCode)ES3.Load("UP", (int)KeyCode.UpArrow));
            KeySetting.keys.Add(KeyAction.DOWN, (KeyCode)ES3.Load("DOWN", (int)KeyCode.DownArrow));
            KeySetting.keys.Add(KeyAction.LEFT, (KeyCode)ES3.Load("LEFT", (int)KeyCode.LeftArrow));
            KeySetting.keys.Add(KeyAction.RIGTH, (KeyCode)ES3.Load("RIGTH", (int)KeyCode.RightArrow));
            KeySetting.keys.Add(KeyAction.ATTACK, (KeyCode)ES3.Load("ATTACK", (int)KeyCode.X));
            KeySetting.keys.Add(KeyAction.JUMP, (KeyCode)ES3.Load("JUMP", (int)KeyCode.Z));
            KeySetting.keys.Add(KeyAction.RESET, (KeyCode)ES3.Load("RESET", (int)KeyCode.R));
        }
    }

    void SoundInitialize()
    {
        bgmVolume = ES3.Load("BgmVolume", 1f);
        effectVolume = ES3.Load("EffectVolume", 1f);
        bgmSoundSlider.value = bgmVolume;
        effectSoundSlider.value = effectVolume;

        MasterAudio.PlaylistMasterVolume = bgmVolume;
        MasterAudio.MasterVolumeLevel = effectVolume;
    }

    void ResolutionInitialize()
    {
        Screen.SetResolution(resolution_Width, resolution_Height, false);
        Resolution_Dropdown_Initialize();
        SetResolution_Dropdown();
    }

    void Resolution_Dropdown_Initialize()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16)
            {
                if (!resolutions.Exists(x => x.width == Screen.resolutions[i].width))
                {
                    resolutions.Add(Screen.resolutions[i]);
                }
            }
        }

        resolutionDropdown.options.Clear();
        resolutions.Reverse();
    }

    void SetResolution_Dropdown()
    {
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.ToString().Split('@')[0];
            resolutionDropdown.options.Add(option);

            if (item.width == resolution_Width)
            {
                resolutionDropdown.value = optionNum;
            }

            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
    }

    public void Resolation_DropboxOptionChange(TMP_Dropdown x)
    {
        resolution_Width = resolutions[x.value].width;
        resolution_Height = resolutions[x.value].height;
    }

    public void OkBtnClick()
    {
        try
        {
            Screen.SetResolution(resolution_Width, resolution_Height, FullScreenMode.Windowed);
            SetPlayerPrefsInfo();
        }
        catch (System.ArgumentException ex)
        {
            Debug.Log(ex);
        }
        optionWindow.SetActive(false);
    }

    public void CloseButtonClick()
    {
        optionWindow.SetActive(false);
    }

    public void KeySettingButton() => keySettingWindow.SetActive(true);

    void SetPlayerPrefsInfo()
    {
        ES3.Save("Resolution_Width", resolution_Width);
        ES3.Save("Resolution_Height", resolution_Height);
        ES3.Save("BgmVolume", MasterAudio.PlaylistMasterVolume);
        ES3.Save("EffectVolume", MasterAudio.MasterVolumeLevel);
    }

    public void ActiveMenuWindow()
    {
        if(menuWindow.activeSelf)
        {
            optionWindow.SetActive(false);
            keySettingWindow.SetActive(false);
        }

        menuWindow.SetActive(!menuWindow.activeSelf);
        Time.timeScale = menuWindow.activeSelf ? 0 : 1;
    }
    public void SettingButton() => optionWindow.SetActive(true);

    public void MainButton() => SceneManager.LoadScene(0);

    public void QuitButton() => Application.Quit();
}
