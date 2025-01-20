using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatformTrap : TrapController
{
    public override void Update()
    {
        base.Update();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(collision.transform.position.y > transform.position.y)
                TrapTriggered();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.transform.position.y > transform.position.y)
                TrapTriggered();
        }
    }
}
