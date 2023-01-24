using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    public GameObject player;
    public float radius;
    public float aimingTime;
    public float speed;
    public float aimingSpeed;
    public float recoveryDistance;

    Vector3 moveUpPos, moveDownPos, attackDir;
    float minDistance = 7f;
    
    bool savePos;
    bool upDown;
    bool isArrival;
    float aimingTimeout;

    AttackMode attackMode;

    void Start()
    {
        savePos = true;
        isArrival = false;
        aimingTimeout = aimingTime;
        attackMode = AttackMode.AIMING;
    }

    // Update is called once per frame
    void Update()
    {
        AimingMotion();
        Attack();
        SpriteFlipX();
    }

    void SpriteFlipX() => GetComponent<SpriteRenderer>().flipX = player.transform.position.x < transform.position.x;

    void AimingMotion()
    {
        if (attackMode != AttackMode.AIMING)
            return;

        if(savePos)
        {
            moveUpPos = transform.position + (Vector3.up * radius);
            moveDownPos = transform.position + (Vector3.down * radius);
            savePos = false;
        }

        //if (upDown)
        //    movePos = moveUpPos;
        //else
        //    movePos = moveDownPos;

        //float dir = Mathf.Sign(movePos.y - transform.position.y);

        //transform.Translate(0, dir * Time.deltaTime * aimingSpeed, 0);

        //if (upDown && transform.position.y >= movePos.y)
        //    upDown = false;
        //else if (!upDown && transform.position.y <= movePos.y)
        //    upDown = true;

        aimingTimeout -= Time.deltaTime;

        if(aimingTimeout <= 0)
        {
            aimingTimeout = aimingTime;
            attackMode = AttackMode.ATTACK;
            attackDir = player.transform.position - transform.position;
        }
    }

    void Attack()
    {
        if (attackMode != AttackMode.ATTACK)
            return;

        transform.Translate(attackDir.normalized * Time.deltaTime * speed);

        if((player.transform.position - transform.position).magnitude < minDistance)
            isArrival = true;

        if (isArrival)
        {
            if ((player.transform.position - transform.position).magnitude > recoveryDistance)
            {
                savePos = true;
                attackMode = AttackMode.AIMING;
                isArrival = false;
            }
        }
    }

    enum AttackMode
    {
        WAITING,
        ATTACK,
        AIMING,
        END
    }
}
