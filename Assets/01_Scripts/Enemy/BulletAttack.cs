using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public float speed;
    public float disapearTime;
    public float dir;

    SpriteRenderer spriteRenderer;
    float time;

    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        time = disapearTime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        AttackMove(dir);
        if (time <= 0)
        {
            time = disapearTime;
            gameObject.SetActive(false);
        }
    }

    public void AttackMove(float direction)
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime * speed, Space.World);
        spriteRenderer.flipX = (direction > 0);
        //transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
    }
}
