using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Rigidbody2D))]
public class TrapController : RaycastController
{
    Rigidbody2D rigid;
    public Vector3 moveDir;

    public float delay;
    public float speed;
    public float disapearTime;

    //Vector3 originPos;
    //Vector3 globalMovePoint;

    //[Range(0, 2)]
    //public float easeAmount;

    //float percentBetweenWaypoints;
    public bool trapTrigger;
    float activeTime;

    bool isFade;
    public override void Start()
    {
        base.Start();
        Initialized();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateRaycastOrigins();

        //if (transform.position == globalMovePoint)
        //    gameObject.SetActive(false);
        if (trapTrigger)
        {
            //Vector3 velocity = CalculatePlatformMovement();
            //transform.Translate(velocity, Space.World);
            rigid.linearVelocity = moveDir * speed;
            boxCollider.isTrigger = true;
            if(disapearTime != -1)
            {
                activeTime+=Time.deltaTime;
                if (activeTime > disapearTime)
                {
                    boxCollider.enabled = false;
                    FadeOutObject();
                }
            }
        }
    }

    void FadeOutObject()
    {
        if (isFade)
            return;
        Sequence sequence = DOTween.Sequence()
                        .Append(GetComponent<SpriteRenderer>().DOFade(0, 0.4f))
                        .OnComplete(() => GetComponent<BoxCollider2D>().enabled = false);
        isFade = true;
    }

    public void Initialized()
    {
        //originPos = transform.position;
        //globalMovePoint = movePos + originPos;

        trapTrigger = false;
        isFade = false;
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        activeTime = 0;
    }

    IEnumerator TrapDelay()
    {
        yield return new WaitForSeconds(delay);
        boxCollider.isTrigger = true;
        trapTrigger = true;
    }
    public void TrapTriggered()
    {
        StartCoroutine(TrapDelay());
    }

    //float Ease(float x) // 닷트윈의 Ease 그래프 참조, 그래프
    //{
    //    float a = easeAmount + 1;
    //    return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    //}

    //Vector3 CalculatePlatformMovement()
    //{
    //    float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]); // 이전장소에서 다음장소 이동 거리 변수
    //    percentBetweenWaypoints += Time.deltaTime * speed;
    //    percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
    //    float easePercentBetweenWaypoints = Ease(percentBetweenWaypoints);

    //    Vector3 newPos = Vector3.Lerp(originPos, globalMovePoint, easePercentBetweenWaypoints);

    //    return newPos - transform.position;
    //}

    private void OnDrawGizmos()
    {
        //if (movePos != null)
        //{
        //    Gizmos.color = Color.red;
        //    float size = .3f;
        //    Vector3 globalWaypointPos = (Application.isPlaying) ? globalMovePoint : movePos + transform.position;
        //    Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
        //    Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
        //}
    }
}
