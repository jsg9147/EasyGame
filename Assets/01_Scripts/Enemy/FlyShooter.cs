using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyShooter : MonoBehaviour
{
    public Transform playerTransform;
    public float speed;
    public float distance;
    public float changePositionTime;
    public BulletAttack[] bullets;
    public Vector3 curvePoint;
    public float shotDelay;

    public float yMoveRange;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    int attackIndex;
    float attackTime;
    float currentTime;

    Vector3 destination, beforePosition, secondPoint;

    bool changeXpos;

    private  void Start()
    {
        attackIndex = 0;
        attackTime = 0;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeXpos = true;
        SetPosition();
    }

    private void FixedUpdate()
    {
        Attack();
        ChaseYPos();
        Move();
        SetDirect();
    }

    public Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    void SetDirect()
    {
        spriteRenderer.flipX = (playerTransform.position.x - transform.position.x) < 0;
    }

    void SetPosition()
    {
        destination = playerTransform.position;
        destination = destination + (Vector3.right * Mathf.Sign(Random.Range(-1, 1)) * distance);
        if (Mathf.Abs(destination.x) > 17f)
            destination.x = destination.x > 0 ? 17f : -17f;

        beforePosition = transform.position;
        secondPoint = playerTransform.position + (curvePoint * Mathf.Sign(Random.Range(-1, 1)));
    }

    void Move()
    {
        if(changeXpos)
        {
            var t = currentTime / speed;
            if (t < 1.0f)
            {
                transform.position = Bezier(t, beforePosition, secondPoint, destination);
            }
            else
            {
                transform.position = destination; //1 or larger means we reached the end
                currentTime = 0;
                changeXpos = !changeXpos;
            }

            currentTime += Time.deltaTime;
        }
    }

    void ChaseYPos()
    {
        if(!changeXpos)
        {
            Vector3 v = destination;
            float dir = playerTransform.position.y - transform.position.y;

            v.x = transform.position.x;
            v.y += yMoveRange * Mathf.Sin(currentTime * speed);

            currentTime += Time.deltaTime;

            transform.position = v;


            if (currentTime >= changePositionTime)
            {
                changeXpos = true;
                currentTime = 0;
                SetPosition();
            }
        }
    }

    void Attack()
    {
        attackTime += Time.deltaTime;

        if(attackTime >= shotDelay)
        {
            bullets[attackIndex].transform.position = transform.position;
            bullets[attackIndex].gameObject.SetActive(true);
            bullets[attackIndex].dir = Mathf.Sign(playerTransform.position.x - transform.position.x);
            attackIndex++;
            attackTime = 0;

            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform("BirdShot", transform);
        }

        if (attackIndex >= bullets.Length)
            attackIndex = 0;
    }
}
