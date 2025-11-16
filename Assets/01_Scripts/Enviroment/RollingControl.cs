using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingControl : MonoBehaviour
{
    public float waitTime;
    public bool isVertical;
    public float rotSpeed;

    Rigidbody2D rigid;
    Vector3 originPos;

    float nextMoveTime;

    bool isDrop;
    private void Start()
    {
        originPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(waitTime != -1)
            PositionReset();

        transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * -1f);
        nextMoveTime += Time.deltaTime;
    }

    void PositionReset()
    {
        if (nextMoveTime < waitTime)
            return;

        if (rigid.linearVelocity == Vector2.zero)
        {
            transform.position = originPos;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            isDrop = false;
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform" && !isDrop)
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound("DropRock");
            isDrop = true;

        }
    }
}
