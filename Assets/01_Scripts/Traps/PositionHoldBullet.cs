using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHoldBullet : MonoBehaviour
{
    Bullet bulletPrefab;

    Bullet[] bullets;

    public int bulletCount;
    public float delay;
    public float speed;

    bool isStart;
    Vector3 dir;
    Vector3 startPos;
    float distance;
    float timeToDelay;
    int index;

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
            Attack();
    }

    void Initialize()
    {
        startPos = transform.position;
        bullets = new Bullet[bulletCount];
        isStart = false;
    }

    public void SetInfo(Bullet _bulletPrefab,Vector3 _dir, float _distance, float _speed = -1)
    {
        bulletPrefab = _bulletPrefab;
        
        dir = _dir.normalized;
        distance = _distance;

        speed = (_speed == -1) ? speed : _speed;

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, transform);
            bullets[i].SetInfo(startPos, dir, speed, distance);
        }
        isStart = true;
    }

    void Attack()
    {
        if(timeToDelay < 0)
        {
            bullets[index].gameObject.SetActive(true);
            index++;

            if (index >= bullets.Length)
                index = 0;

            timeToDelay = delay;
        }

        timeToDelay -= Time.deltaTime;

    }

    public void AttackEnd()
    {
        isStart = false;
        gameObject.SetActive(false);
    }
}
