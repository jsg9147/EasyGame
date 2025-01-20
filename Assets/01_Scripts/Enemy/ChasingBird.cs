using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
public class ChasingBird : MonoBehaviour
{
    public GameObject player;
    public GameObject door;

    public float speed;
    public float arrivalTime;

    float currentTime;
    public Vector3 beforePosition, secondPoint, destination, direct;

    SpriteRenderer spriteRenderer;

    bool isCurve;
    float straightTime = 2f;

    bool isStart;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = 0;
        SetMovePoint();
        isCurve = false;
        isStart = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!door.activeSelf)
        {
            Move();
            spriteRenderer.flipX = (Direct().x > 0);
            spriteRenderer.sortingOrder = 1;

            if(!isStart)
            {
                MasterAudio.PlaySound3DAtTransform("CrowStart", transform);
                isStart = true;
            }
        }
        
    }
    public Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    void Move()
    {   
        if(currentTime < straightTime)
        {
            if (isCurve)
            {
                isCurve = false;
                MasterAudio.PlaySound("CrowAttack");
            }
            StraightMove();
        }
        else
        {
            if(!isCurve)
            {
                SetMovePoint();
                isCurve = true;
            }

            CurveMove();
        }

        currentTime += Time.deltaTime;
    }

    void CurveMove()
    {
        float curveTime = currentTime - straightTime;
        var t = curveTime / arrivalTime;
        if (t < 1.0f)
        {
            transform.position = Bezier(t, beforePosition, secondPoint, destination);
        }
        else
        {
            transform.position = destination; //1 or larger means we reached the end
            SetMovePoint();
            currentTime = 0;
        }
    }

    void StraightMove()
    {
        if (Mathf.Abs(transform.position.x) < 18f)
            transform.Translate(direct * speed * Time.deltaTime * 3f);
    }

    void SetMovePoint()
    {
        float dir = Mathf.Sign(direct.x);
        beforePosition = transform.position;
        destination = EndPoint();

        secondPoint = destination + (Vector3.up * (Mathf.Sign(Random.Range(-1,1)) * 5));

        direct = Direct();
    }

    Vector3 EndPoint()
    {
        float dir = Mathf.Sign(direct.x);
        float xPos = transform.position.x + (4 * dir);
        if(Mathf.Abs(xPos) > 16f)
        {
            xPos = 16f * dir;
        }

        return new Vector3(xPos, player.transform.position.y + 10, 0);
    }
    Vector3 Direct()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir.normalized;
    }
}
