using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum KeyAction { UP, DOWN, LEFT, RIGTH, ATTACK, JUMP, RESET, KEYCOUNT };
public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }
public class KeyManager : MonoBehaviour
{
    public TMP_Text[] txt;
    Button selectedButton;
    int key;

    private void Start()
    {
        key = -1;
        for (int i = 0; i< txt.Length; i++)
        {
            txt[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }

    private void Update()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if(keyEvent.isKey)
        {
            if(keyEvent.keyCode == KeyCode.Escape)
            {
                selectedButton = null;
                key = -1;
                return;
            }

            if (selectedButton)
                selectedButton.GetComponent<Image>().color = Color.white;

            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;

            selectedButton = null;
            key = -1;
        }
    }

    public void ChangeKey(int num)
    {
        key = num;
    }

    public void SelectKeyChange(Button button)
    {
        if (selectedButton)
            selectedButton.GetComponent<Image>().color = Color.white;

        selectedButton = button;
        button.GetComponent<Image>().color = Color.gray;
    }
    public void KeySave()
    {
        ES3.Save("UP", (int)KeySetting.keys[KeyAction.UP]);
        ES3.Save("DOWN", (int)KeySetting.keys[KeyAction.DOWN]);
        ES3.Save("LEFT", (int)KeySetting.keys[KeyAction.LEFT]);
        ES3.Save("RIGTH", (int)KeySetting.keys[KeyAction.RIGTH]);
        ES3.Save("ATTACK", (int)KeySetting.keys[KeyAction.ATTACK]);
        ES3.Save("JUMP", (int)KeySetting.keys[KeyAction.JUMP]);
        ES3.Save("RESET", (int)KeySetting.keys[KeyAction.RESET]);
        gameObject.SetActive(false);
    }

    public void UnSave()
    {
        KeySetting.keys[KeyAction.UP] = (KeyCode)ES3.Load("UP", (int)KeyCode.UpArrow);
        KeySetting.keys[KeyAction.DOWN] = (KeyCode)ES3.Load("DOWN", (int)KeyCode.DownArrow);
        KeySetting.keys[KeyAction.LEFT] = (KeyCode)ES3.Load("LEFT", (int)KeyCode.LeftArrow);
        KeySetting.keys[KeyAction.RIGTH] = (KeyCode)ES3.Load("RIGTH", (int)KeyCode.RightArrow);
        KeySetting.keys[KeyAction.ATTACK] = (KeyCode)ES3.Load("ATTACK", (int)KeyCode.X);
        KeySetting.keys[KeyAction.JUMP] = (KeyCode)ES3.Load("JUMP", (int)KeyCode.Z);
        KeySetting.keys[KeyAction.RESET] = (KeyCode)ES3.Load("RESET", (int)KeyCode.R);
        gameObject.SetActive(false);
    }
}
