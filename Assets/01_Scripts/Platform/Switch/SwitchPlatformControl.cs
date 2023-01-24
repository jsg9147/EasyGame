using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SwitchPlatformControl : MonoBehaviour
{
    public Sprite switchOnSprite;
    public Sprite switchOffSprite;

    public GameObject[] onSwitchPlatforms;
    public GameObject[] offSwitchPlatforms;

    SpriteRenderer spriteRenderer;
    bool isOn;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isOn = false;
        SwitchPlatforms();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack")
            SwitchPlatforms();
    }

    void SwitchPlatforms()
    {
        isOn = !isOn;
        spriteRenderer.sprite = isOn ? switchOnSprite : switchOffSprite;
        foreach(GameObject platform in onSwitchPlatforms)
        {
            platform.SetActive(isOn);
        }

        foreach(GameObject platform in offSwitchPlatforms)
        {
            platform.SetActive(!isOn);
        }
    }
}
