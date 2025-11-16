using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectPlatform : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    Rigidbody2D rigid;

    void Start()
    {
        direction = direction.normalized;
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.linearVelocity = direction * speed;
    }
}
