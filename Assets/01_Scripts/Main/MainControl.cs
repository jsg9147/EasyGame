using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;

public class MainControl : MonoBehaviour
{
    public GameObject optionWindow;
    public GameObject infoWindow;

    public void NewGameButton()
    {
        ES3.Save("PlayerX", 0f);
        ES3.Save("PlayerY", 0f);
        ES3.Save("StageIndex", 1);
        ES3.Save("DeathCount", 0);
        ES3.Save("ResetCount", 0);

        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        int stageIndex = ES3.Load("StageIndex", 1);
        SceneManager.LoadScene(stageIndex);
    }

    public void SettingButton()
    {
        optionWindow.SetActive(!optionWindow.activeSelf);
    }

    public void InfoButton()
    {
        infoWindow.SetActive(!infoWindow.activeSelf);
    }
    public void ExitButton() => Application.Quit();
}
