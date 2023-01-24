using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : Enemy
{
    public float speed;
    public float dashSpeed;

    [HideInInspector]
    public int nextMove;
    private void Awake()
    {
        Init();

        Invoke("Think", 3);
    }

    private void FixedUpdate()
    {
        if(!isDie)
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
        rigid.velocity = new Vector2(xSpeed, rigid.velocity.y);
        //Platform Chaeck
        Vector2 frontVec = new Vector2(rigid.position.x + (nextMove * 0.7f), rigid.position.y);
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
        if(nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        //Recursive
        float nextThinkTime = Random.Range(0.5f, 1.2f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        CancelInvoke();
        Invoke("Think", 1);
    }
}
