using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : RaycastController
{
    public GameObject trap;
    Collider2D detectCollider;
    public bool isVertical;
    public Vector2 direction;
    public float detectingDistance;

    bool isTrigger;
    public override void Start()
    {
        base.Start();
        detectCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        UpdateRaycastOrigins();

        if(isTrigger)
            trap.SetActive(true);

        DetectedPlayer();
    }

    void DetectedPlayer()
    {
        if (isTrigger)
            return;

        float directionX = Mathf.Sign(direction.x);
        float directionY = Mathf.Sign(direction.y);
        if (isVertical)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, detectingDistance, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

                if (hit)
                {
                    if (hit.collider.tag == "Player")
                    {
                        isTrigger = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, detectingDistance, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

                if (hit)
                {
                    if (hit.collider.tag == "Player")
                    {
                        isTrigger = true;
                    }
                }
            }
        }
    }
}
