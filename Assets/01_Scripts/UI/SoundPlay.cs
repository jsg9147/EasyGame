using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
public class SoundPlay : MonoBehaviour
{
    public string soundName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MasterAudio.PlaySound3DAtTransform(soundName, transform);
    }

    
}
