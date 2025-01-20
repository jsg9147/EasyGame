using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class Tornado : MonoBehaviour
{
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void EndEffect() => gameObject.SetActive(false);

    public void PlaySound() => MasterAudio.PlaySound("Tornado");
}
