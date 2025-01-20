using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.position.y > transform.position.y && collision.gameObject.tag == "Player")
        {
            anim.Play("Spring Cloud", -1, 0f);
        }
    }
}
