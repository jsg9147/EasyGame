using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(BoxCollider2D))]
public class Switch : MonoBehaviour
{
    public GameObject door;
    public Sprite openLever;
    public float disapearTime;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    
    bool isActive;
    float time;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if(isActive)
        {
            OpenDoor();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            ActiveSwitch();
        }
    }

    void ActiveSwitch()
    {
        if(!isActive)
            MasterAudio.PlaySound3DAtTransform("SwitchDoor", transform);
        spriteRenderer.sprite = openLever;
        isActive = true;
    }

    void OpenDoor()
    {
        door.transform.Translate(Vector2.down * Time.deltaTime);

        time += Time.deltaTime;

        if (time >= disapearTime)
        {
            door.SetActive(false);
            
        }
    }
}
