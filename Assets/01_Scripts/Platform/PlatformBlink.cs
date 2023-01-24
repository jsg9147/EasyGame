using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlink : MonoBehaviour
{
    public GameObject[] blinkObjs;

    public float delayTime;

    float currentTime;

    private void Start()
    {
        currentTime = delayTime;
    }

    private void Update()
    {
        Blink();
    }

    void Blink()
    {
        if(currentTime <= 0)
        {
            foreach(GameObject gameObject in blinkObjs)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }

            currentTime = delayTime;
        }

        currentTime -= Time.deltaTime;
    }
}
