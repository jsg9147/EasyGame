using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePlatformContoller : MonoBehaviour
{
    public Vector3[] localWaypoints;
    public float watiTime;
    public float speed;
    public int bodyCount;

    public GameObject platform;
    public GameObject head;
    public GameObject tail;

    GameObject[] platforms;

    Vector3 nextMovePos;
    Vector3 nextDestination;
    int tailIndex, fromWaypointIndex;
    bool isMove, isComplite;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (isMove)
        {
            HeadMove();
            TailMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            StartCoroutine(MoveStart());
    }

    IEnumerator MoveStart()
    {
        yield return new WaitForSeconds(watiTime);
        isMove = true;
    }

    void Initialize()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(bodyCount + 1, 1);
        GetComponent<BoxCollider2D>().offset = new Vector2( -(bodyCount / 2.0f), 0);

        tailIndex = bodyCount - 1;
        fromWaypointIndex = 0;
        platforms = new GameObject[bodyCount];
        isMove = false;


        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i] = Instantiate(platform, transform);
            platforms[i].transform.position = head.transform.position + (Vector3.left * i);
            platforms[i].name = i + " platform";
        }

        tail.transform.position = head.transform.position + (Vector3.left * platforms.Length);
        nextDestination = head.transform.position + localWaypoints[fromWaypointIndex];
        nextMovePos = head.transform.position + localWaypoints[fromWaypointIndex].normalized;
    }

    void HeadMove()
    {
        if (isComplite)
            return;

        head.transform.position = Vector3.MoveTowards(head.transform.position, nextMovePos, Time.deltaTime * speed);

        if(head.transform.position == nextMovePos)
        {
            MoveToHead();

            if (nextMovePos == nextDestination)
            {
                fromWaypointIndex++;
                if (fromWaypointIndex < localWaypoints.Length)
                {
                    nextDestination = head.transform.position + localWaypoints[fromWaypointIndex];
                    nextMovePos = head.transform.position + localWaypoints[fromWaypointIndex].normalized;
                }
                else
                    isComplite = true;
            }
            else
            {
                nextMovePos = head.transform.position + localWaypoints[fromWaypointIndex].normalized;
            }
        }
    }

    void MoveToHead()
    {
        platforms[tailIndex].transform.position = head.transform.position;
        tailIndex--;
        if (tailIndex < 0)
        {
            tailIndex = bodyCount - 1;
        }
    }

    void TailMove()
    {
        if (isComplite)
            return;

        tail.transform.position = Vector3.MoveTowards(tail.transform.position, platforms[tailIndex].transform.position, Time.deltaTime * speed);
    }
}
