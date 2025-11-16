using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : Enemy
{
    GameObject throwPrefab;
    GameObject[] throwObjs;
    public float delay;
    public float disapearTime;
    public float minPower;
    public float maxPower;

    int attackIndex;
    float time;
    public override void Start()
    {
        base.Start();
        foreach (GameObject throwObj in throwObjs)
        {
            throwObj.GetComponent<ThrowObj>().disapearTime = disapearTime;
            throwObj.SetActive(false);
        }
        Attack();
        time = disapearTime;
    }

    void Attack()
    {
        float xDir = spriteRenderer.flipX ? -1 : 1;
        float xPower = Random.Range(minPower, maxPower);
        float yPower = Random.Range(minPower, maxPower);

        throwObjs[attackIndex].transform.position = transform.position + (Vector3.up * 0.5f);
        throwObjs[attackIndex].SetActive(true);
        throwObjs[attackIndex].GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xDir * xPower, yPower);

        attackIndex++;

        if (attackIndex >= throwObjs.Length)
            attackIndex = 0;

        Invoke("Attack", delay);
    }


}
