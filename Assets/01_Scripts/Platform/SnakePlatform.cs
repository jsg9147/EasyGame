using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnakePlatform : MonoBehaviour
{
    public Transform head;
    public Transform[] tails;
    public Vector3[] localWaypoints;
    public float waitTime;
    public float speed;
    public float size;

    Vector3[] globalWaypoints;
    int fromWaypointIndex;
    int[] tailWaypoints;

    bool isMove;

    BoxCollider2D boxCollider;

    Vector3 before = Vector3.zero;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if(isMove)
        {
            HeadMove();
            TailMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("ActivePlatform", waitTime);
        }
    }

    void ActivePlatform()
    {
        isMove = true;
    }

    void Initialize()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        tailWaypoints = new int[tails.Length];
        fromWaypointIndex = 0;
        boxCollider = GetComponent<BoxCollider2D>();
        isMove = false;

        boxCollider.size = new Vector2((tails.Length + 1) * size, size);
        boxCollider.offset = new Vector2(-(size * tails.Length)/2, 0);

        for(int i = 0; i < tailWaypoints.Length; i++)
        {
            tailWaypoints[i] = 0;
        }
        
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + head.position;
        }
    }

    void HeadMove()
    {
        if (fromWaypointIndex >= globalWaypoints.Length)
            return;
        Vector3 dir = globalWaypoints[fromWaypointIndex] - head.position;
        before = dir.normalized * Time.deltaTime * speed;
        head.Translate(dir.normalized * Time.deltaTime * speed, Space.World);

        if (Mathf.Abs(dir.x) <= .05f && Mathf.Abs(dir.y) <= .05f)
        {
            head.position = globalWaypoints[fromWaypointIndex];
            fromWaypointIndex++;
        }
    }

    void TailMove()
    {
        if (fromWaypointIndex >= globalWaypoints.Length)
            return;

        for (int i =0; i< tails.Length; i++)
        {
            if (tailWaypoints[i] > globalWaypoints.Length - 1)
                continue;

            Vector3 dir = globalWaypoints[tailWaypoints[i]] - tails[i].position;

            if (tailWaypoints[i] == globalWaypoints.Length -1)
            {
                float lastPos = (size * (1 + i));
                if (Mathf.Abs(dir.x) >= .05f)
                {
                    dir.x = dir.x - lastPos * Mathf.Sign(dir.x) ;
                }
                else if (Mathf.Abs(dir.y) >= .05f)
                {
                    dir.y = dir.y - lastPos * Mathf.Sign(dir.y);
                }
            }

            tails[i].Translate(dir.normalized * Time.deltaTime * speed, Space.World);
            
            if (Mathf.Abs(dir.x) <= .05f && Mathf.Abs(dir.y) <= .05f)
            {
                tails[i].position = globalWaypoints[tailWaypoints[i]];
                tailWaypoints[i]++;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }
}
