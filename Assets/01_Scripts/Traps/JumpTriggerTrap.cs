using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerTrap : MonoBehaviour
{
    public bool isActive;
    public float speed;

    public float time;

    Vector3 activePosition;
    Vector3 inactivePosition;
    bool isMove;
    void Start()
    {
        activePosition = isActive ? transform.localPosition : transform.localPosition + Vector3.up;
        inactivePosition = isActive ? transform.localPosition + Vector3.down : transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]))
        {
            isMove = true;
            isActive = !isActive;
        }

        TrapMove();
    }

    void TrapMove()
    {
        if (isMove)
        {
            Vector3 direct = isActive ? Vector3.up : Vector3.down;
            Vector3 movePosition = isActive ? activePosition : inactivePosition;

            transform.Translate(direct * speed * Time.deltaTime);
            if (isActive && transform.localPosition.y - movePosition.y >= 0f)
                isMove = false;

            if (!isActive && transform.localPosition.y - movePosition.y <= 0f)
                isMove = false;
        }
    }
}
