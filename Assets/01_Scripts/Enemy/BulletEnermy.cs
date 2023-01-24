using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnermy : MonoBehaviour
{
    public BulletAttack bullet;
    public float delay;

    int bulletCount = 10;
    BulletAttack[] bullets;

    int index;
    float time;
    private void Start()
    {
        time = delay;
        bullets = new BulletAttack[10];
        for(int i = 0; i< bulletCount; i++)
        {
            bullets[i] = Instantiate(bullet, transform);
        }
    }

    private void Update()
    {
        ShootBullet();
    }


    void ShootBullet()
    {
        if (index >= bullets.Length)
            index = 0;

        if(time <= 0f)
        {
            bullets[index].transform.localPosition = Vector3.zero;
            bullets[index].gameObject.SetActive(true);
            index++;
            time = delay;
        }

        time -= Time.deltaTime;

    }
}
