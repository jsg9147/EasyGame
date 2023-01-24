using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDetectPlatform : PlatformController
{
    public float firstWaitTime;
    public bool isRepetition;

    // 나중에 rigidbody 로 이동하게 변경하자. 충돌 여부 변경시키면 가능할꺼 같음, 지금은 일딴 기능만 추가
    public bool disapearObject;

    bool onPlayer;
    bool firstWait;

    int beforeIndex;

    bool isDisapear;
    public override void Start()
    {
        base.Start();

        firstWait = true;
        beforeIndex = 0;
        isDisapear = false;
        onPlayer = false;

        if(GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().freezeRotation = true;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if(IsMove())
        {
            base.Update();
        }
        if (isComplite && disapearObject)
        {
            Disapear();
        }
    }

    bool IsMove()
    {
        if (isComplite)
            return false;
        bool isMove = false;
        if (onPlayer)
        {
            gameObject.layer = 8;
            isMove = true;
        }
        if (firstWait && isMove)
        {
            nextMoveTime = Time.time + firstWaitTime;
            firstWait = false;
        }

        if (beforeIndex > fromWaypointIndex)
            isMove = false;
        
        if(beforeIndex < fromWaypointIndex)
            beforeIndex = fromWaypointIndex;

        return isMove;
    }

    void Disapear()
    {
        if (isDisapear)
            return;

        gameObject.SetActive(false);
        isDisapear = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPlayer = true;
    }
}
