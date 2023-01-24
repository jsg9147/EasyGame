using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class SoundEvent : MonoBehaviour
{
    public void PointerEnter() => MasterAudio.PlaySound("Button_Enter_Mouse_Sound");
    public void ButtonClick() => MasterAudio.PlaySound("Button_Click_Sound");
}
