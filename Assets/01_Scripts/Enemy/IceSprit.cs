using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSprit : Enemy
{
    public float speed;
    public float dashSpeed;

    [HideInInspector]
    public int nextMove;

    private void Awake()
    {
        Init();

        Invoke("Think", 1);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Move()
    {
        float xSpeed = detected ? nextMove * dashSpeed : nextMove * speed;
        rigid.linearVelocity = new Vector2(xSpeed, rigid.linearVelocity.y);
        //Platform Chaeck
        Vector2 frontVec = new Vector2(rigid.position.x + (nextMove * 0.5f), rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, collisionMask);

        if (rayHit.collider == null)
        {
            Turn();
        }
    }
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite
        if (nextMove != 0)
        {
            transform.rotation = nextMove == 1 ? new(0, 180, 0, 0) : new(0, 0, 0, 0);
        }

        //Recursive
        float nextThinkTime = Random.Range(1f, 2f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;

        if (nextMove != 0)
        {
            transform.rotation = nextMove == 1 ? new(0, 180, 0, 0) : new(0, 0, 0, 0);
        }

        CancelInvoke();
        Invoke("Think", 1);
    }

    public override void Death()
    {
        anim.SetBool("isDie", true);
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform("IceSpritDeath", transform);
        StartCoroutine(DeActive(1f));
    }

    IEnumerator DeActive(float delay)
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
