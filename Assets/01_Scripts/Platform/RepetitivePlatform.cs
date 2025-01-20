using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetitivePlatform : PlatformController
{
    public float startupDelay;
    public override void Start()
    {
        cyclic = true;
        base.Start();
        nextMoveTime = Time.time + startupDelay;
    }
    public override void Update()
    {
        //if (fromWaypointIndex >= localWaypoints.Length - 1)
        //{
        //    transform.position = localWaypoints[0] + transform.position;
        //    fromWaypointIndex = 0;
        //}
        base.Update();
    }
}
