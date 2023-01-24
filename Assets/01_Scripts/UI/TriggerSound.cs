using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public string clipString;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Platform")
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(clipString, transform);
    }
}
