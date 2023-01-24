using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyVelocity : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private Vector2 direction;
    float speed;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid.gravityScale = 0;

        if (direction.x < 0)
            spriteRenderer.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Platform")
        {
            gameObject.SetActive(false);
        }
    }

    public void SetSpeed(float speed) => this.speed = speed;
    public void SetDirectionAndRotate(Vector3 value)
    {
        Vector3 direct = value - transform.position;
        direction = direct.normalized;
        SetRotate();
    }

    public void SetDirect(Vector3 direct) => direction = direct.normalized;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    void SetRotate()
    {
        transform.rotation = Quaternion.Euler(new (0, 0, GetAngle()));
    }

    float GetAngle()
    {
        float angle = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
        return angle;
    }
}
