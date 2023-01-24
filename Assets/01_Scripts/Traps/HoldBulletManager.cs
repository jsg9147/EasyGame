using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBulletManager : MonoBehaviour
{
    public Bullet bulletPrefab;
    public PositionHoldBullet[] bullets;
    public Vector3 dir;
    public float distance;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInToArea();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerExitArea();
    }

    void PlayerInToArea()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetInfo(bulletPrefab, dir, distance);
            bullets[i].gameObject.SetActive(true);
        }
    }

    void PlayerExitArea()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].AttackEnd();
        }
    }
}
