using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class RepetitiveMove : MonoBehaviour
{
    public Vector2 moveDir;
    public float speed;
    public float resetTime;
    public float startDelay;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Vector3 originPos;
    float time;
    bool wait;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        originPos = transform.position;
        time = startDelay;

        rigid.gravityScale = 0;
        wait = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            wait = true;
            transform.position = originPos;
        }
    }

    void Move()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            transform.position = originPos;
            time = resetTime;
            wait = false;
        }

        if (wait)
            rigid.velocity = Vector2.zero;
        else
            rigid.velocity = moveDir * speed;

    }
}
