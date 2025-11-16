using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Javelin : MonoBehaviour
{
    Player player;

    public float damage;
    public Vector2 dir;
    public float speed;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;

    float cameraWidthSize;
    float cameraHeightSize;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigid.gravityScale = 0;
        boxCollider.isTrigger = true;

        cameraHeightSize = Camera.main.orthographicSize;
        cameraWidthSize = cameraHeightSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        ThorwToDirection();
        MaxRangeReset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "Enemy" || collision.tag == "Switch")
        {
            if(collision.tag == "Enemy")
                MasterAudio.PlaySound3DAtTransform("Hit", transform);

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
            gameObject.SetActive(false);
    }

    void ThorwToDirection()
    {
        rigid.linearVelocity = dir * speed;
    }
    void MaxRangeReset()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float maxXposition = cameraWidthSize + 10f;
        float xPosition = transform.position.x;
        if(xPosition < cameraPosition.x - maxXposition || xPosition > cameraPosition.x + maxXposition)
            gameObject.SetActive(false);
    }

    public void SetDirect(Vector2 direct)
    {
        this.dir = direct;
        GetComponent<SpriteRenderer>().flipX = dir.x == -1;
    }

    public void SetPlayer(Player player) => this.player = player;

    public void SetPosition(Transform hand)
    {
        transform.position = hand.position;
    }

}
