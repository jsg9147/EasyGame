using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class SetActiveControl : MonoBehaviour
{
    public void SetActiveFalse() => gameObject.SetActive(false);
    public void Play() => MasterAudio.PlaySound("Fire");
}
