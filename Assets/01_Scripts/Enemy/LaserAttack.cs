using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserAttack : MonoBehaviour
{
    public Camera mainCamera;

    public SpriteRenderer handEffect;
    public SpriteRenderer attack;
    public float apearSpeed;

    public float attackYPos;
    public float apearTime;
    
    public float spriteSize;

    public float activeTime;

    bool isArrival;
    bool activeAttack;

    Vector2 enemyMoveDistance;

    private void Start()
    {
        enemyMoveDistance = Vector2.zero;
        isArrival = false;
        activeAttack = false;
    }
    private void FixedUpdate()
    {
        MaintainDistance();
        ApearEnemy();
    }

    void MaintainDistance()
    {
        if(mainCamera.transform.position.y >= attackYPos)
            isArrival = true;
    }

    void ApearEnemy()
    {
        if (!isArrival)
        {
            float distance = mainCamera.orthographicSize + spriteSize;

            Vector2 cameraPos = mainCamera.transform.position;
            {
                transform.position = new Vector3(transform.position.x, (cameraPos + (Vector2.up * distance)).y, 0);
            }
        }
        else
        {
            float distance = mainCamera.orthographicSize + spriteSize;
            Vector2 cameraPos = mainCamera.transform.position;
            if (transform.position.y <= cameraPos.y + mainCamera.orthographicSize - 2f)
            {
                Attack();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (cameraPos + (Vector2.up * distance) - enemyMoveDistance).y, 0);
                enemyMoveDistance = enemyMoveDistance + (Vector2.up * Time.deltaTime * apearSpeed);
            }
        }
    }

    void Attack()
    {
        
        attack.gameObject.tag = "Enemy";
        if(!activeAttack)
        {
            Sequence attackSequence = DOTween.Sequence()
            .Append(attack.transform.DOScaleY(1f, activeTime));
            Sequence attackSequence2 = DOTween.Sequence()
            .Append(attack.transform.DOLocalMoveY(-32f, activeTime))
            .AppendCallback(() =>
            {
                gameObject.SetActive(false);
            });

        }
        activeAttack = true;
    }
}
