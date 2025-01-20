using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControl : MonoBehaviour
{
    public SwitchManager switchManager;
    [HideInInspector]
    public SpriteRenderer switchSprite;

    private void Start()
    {
        switchSprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
            switchManager.SwitchOperate();
    }
}
