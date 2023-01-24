using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrap : TrapController
{
    public float detectingDistance;
    bool isFire = false;
    public override void Update()
    {
        base.Update();
        DetectedPlayer();
    }

    void DetectedPlayer()
    {
        float directionX = Mathf.Sign(moveDir.x);
        float directionY = Mathf.Sign(moveDir.y);

        if (moveDir.y != 0)
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
                        TrapTriggered();
                    }
                }
            }
        }
        
        if(moveDir.x != 0)
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
                        TrapTriggered();
                    }
                }
            }
        }
        if (trapTrigger && !isFire)
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound("TrapFire");
            isFire = true;
        }
    }
}
