using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask playerCollisionMask;

    public int life = 1;
    public float detectingDistance;

    public Animator anim;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public bool isDie;
    public bool detected;
    Color originColor;

    public virtual void Start()
    {
        Initialization();
    }

    public virtual void LateUpdate()
    {
        DetectedPlayer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
            OnDamaged();
    }

    void Initialization()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originColor = spriteRenderer.color;
    }

    public virtual void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 0.7f, 0.7f);
        
        life--;

        if (life <= 0)
            Death();
        else
            StartCoroutine(RecoveryColor());
    }
    IEnumerator RecoveryColor()
    {
        yield return new WaitForSeconds(0.05f);
        if(!isDie)
            spriteRenderer.color = originColor;
    }

    public virtual void Death()
    {
        isDie = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        boxCollider.enabled = false;
        rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
        this.gameObject.layer = 15;
        Invoke("DeActive", 0.7f);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void DetectedPlayer()
    {
        int directionX = spriteRenderer.flipX ? 1 : -1;
        Vector2 rayOrigin = transform.position;
        rayOrigin.y = rayOrigin.y - 0.5f;// 지금 크기문제로 레이저가 약간 위인거 같음
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, detectingDistance, playerCollisionMask);
        
        //Debug.DrawRay(rayOrigin, Vector2.right * directionX * detectingDistance, Color.red);

        if (hit)
            detected = hit.collider.tag == "Player";
        else
            detected = false;
    }
}
