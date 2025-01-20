using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerChaser : MonoBehaviour
{
    public GameObject player;
    public GameObject door;
    public float speed;
    public float maxSpeed;
    public float minDistance; 
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector3 startPosition;
    Vector3 endPosition;

    float randomError = 0.5f;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        endPosition = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!door.activeSelf)
        {
            spriteRenderer.sortingOrder = 0;
            ChasingPlayer();
        }

    }

    void ChasingPlayer()
    {
        if (AwayFromPlayer())
            SetPositionInfomation();

        rigid.AddForce(MoveDirection() * speed);
        MaxSpeedLimit();

        spriteRenderer.flipX = rigid.velocity.x > 0f;
    }
    bool AwayFromPlayer() => (player.transform.position - transform.position).magnitude > minDistance;

    void SetPositionInfomation()
    {
        startPosition = transform.position;
        endPosition = RandomAttackPoint();
    }
    Vector3 RandomAttackPoint() => player.transform.position + new Vector3(Random.Range(-randomError, randomError), Random.Range(-randomError, randomError), 0);

    Vector3 MoveDirection() => (endPosition - startPosition).normalized;

    void MaxSpeedLimit()
    {
        if (rigid.velocity.magnitude > maxSpeed)
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
    }
}
