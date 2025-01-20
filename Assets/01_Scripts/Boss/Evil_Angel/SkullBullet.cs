using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class SkullBullet : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private Vector2 direction;
    float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
        rigid.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = direction * speed;
        AnimationChange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "Player")
            Explosion();
    }
    void Explosion()
    {
        speed = 0;
        SetTrigger("Explosion");
        anim.SetInteger("Direct", 0);
        MasterAudio.PlaySound("Fire");
    }

    void SetTrigger(string triggerName) => anim.SetTrigger(triggerName);

    public void SetSpeed(float speed) => this.speed = speed;
    public void SetDirectionAndRotate(Vector3 value)
    {
        Vector3 direct = value - transform.position;
        direction = direct.normalized;
    }

    void AnimationChange()
    {
        float angle = GetAngle() + 22.5f;
        int direct = (int)(angle / 45f);
        if (direct == 0)
            direct = 8;

        anim.SetInteger("Direct", direct);
    }

    public void SetDirect(Vector3 direct) => direction = direct.normalized;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void DisableObject() => gameObject.SetActive(false);

    void SetRotate()
    {
        transform.rotation = Quaternion.Euler(new(0, 0, GetAngle()));
    }

    float GetAngle()
    {
        float angle = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
        if (angle < 0)
            angle += 360f;
        return angle;
    }
}
