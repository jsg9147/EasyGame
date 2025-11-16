using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RepetitiveTrap : MonoBehaviour
{
    public float speed;
    public Vector3[] localWaypoints;
    public float startDelay;
    Vector3[] globalWaypoints;
    int fromWaypointIndex;

    float minMoveDistance;
    float time;
    Rigidbody2D rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        fromWaypointIndex = 0;
        minMoveDistance = speed * 0.01f;
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
        time = 0;
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if(time > startDelay)
            Move();
    }

    void Move()
    {
        Vector3 dir = globalWaypoints[fromWaypointIndex] - transform.position;

        rigid.linearVelocity = (dir.normalized * speed);
        if (Mathf.Abs(dir.x) <= minMoveDistance && Mathf.Abs(dir.y) <= minMoveDistance)
        {
            transform.position = globalWaypoints[fromWaypointIndex];
            fromWaypointIndex++;
            if (fromWaypointIndex >= globalWaypoints.Length)
            {
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
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
}
