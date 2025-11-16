using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(Rigidbody2D))]
public class BlueDragon : Boss
{
    public Sprite landOnDamagedSprite;
    public Sprite flyingIdleOnDamagedSprite;
    public Sprite flyingOnDamagedSprite;

    public RigidbodyVelocity spitIce;
    public GameObject iceNiddle;
    RigidbodyVelocity[] spitIces;
    GameObject[] iceNiddles;
    int currentSpitIndex;
    int currentNiddleIndex;

    public GameObject spitPosition;
    public GameObject niddleDropPosition;

    public LayerMask collisionMask;
    public float speed;
    public float maxSpeed;
    public float thinkTime;

    public float spitSpeed;

    Rigidbody2D rigid;

    int direct;
    float originPosition, moveDestination;

    int flyingCount, groundCount;

    //

    public float maxXposition;
    public float maxYposition, minYposition;

    bool isIdle , isWalk, isFlying, takeOff, isLanding, attackMotion;

    bool isTop
    {
        get
        {
            return transform.position.y > maxYposition;
        }
    }

    bool isLand
    {
        get
        {
            return transform.position.y < minYposition;
        }
    }

    public override void Start()
    {
        base.Start();
        Setup();
        Idle();
    }

    void Setup()
    {
        rigid = GetComponent<Rigidbody2D>();
        isIdle = false;
        direct = -1;
        spitIces = new RigidbodyVelocity[10];
        iceNiddles = new GameObject[100];
        currentSpitIndex = 0;

        groundCount = 0;
        flyingCount = 0;
        ObjectArrayCreate();
    }

    void ObjectArrayCreate()
    {
        for(int i =0; i< spitIces.Length; i++)
        {
            spitIces[i] = Instantiate(spitIce);
            spitIces[i].transform.position = transform.position;
            spitIces[i].SetSpeed(spitSpeed);
            spitIces[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < iceNiddles.Length; i++)
        {
            iceNiddles[i] = Instantiate(iceNiddle);
            iceNiddles[i].transform.position = transform.position;
            iceNiddles[i].SetActive(false) ;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack")
            OnDamaged(collision.GetComponent<Javelin>());
    }

    public override void OnDamaged(Javelin javelin)
    {
        base.OnDamaged(javelin);
        SetProgressBar();
    }

    void SetRotate()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = (direct > 0) ? new (rot.x, 0, rot.z, 0) : new(rot.x, 180, rot.z, 0);
    }

    public void Idle()
    {
        if (isIdle || attackMotion)
            return;

        isIdle = true;
        SetTrigger("isIdle");
        anim.ResetTrigger("isBreathing");
        rigid.linearVelocity = Vector2.zero;
        direct = (int)Mathf.Sign(PlayerPosition().x - transform.position.x);
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkTime);
        SetRotate();
        anim.ResetTrigger("isIdle");
        int random = Random.Range(0, 3);
        if (groundCount > 1)
            random = 2;
        switch (random)
        {
            case 0:
                Walk();
                break;
            case 1:
                Breath();
                break;
            case 2:
                Flying();
                break;
        }
        isIdle = false;
        if (random < 2)
            groundCount++;
    }
    void Walk()
    {
        isWalk = true;
        SetTrigger("isWalk");
        SetRotate();
        SetWalkDestination();
    }
    void SetWalkDestination()
    {
        originPosition = transform.position.x;
        moveDestination = originPosition + (Random.Range(5, 20) * direct);

        if (Mathf.Abs(moveDestination) > maxXposition)
            moveDestination = maxXposition * direct;
    }

    void SetVelocity()
    {
        SetHorizentalVelocity();
        SetVerticalVelocity();
    }

    void SetHorizentalVelocity()
    {
        Calculate_Walk_Horizental_Velocity();
        Calculate_Headbutt_Horizental_Velocity();
        Calculate_Flying_Horizental_Velocity();
    }

    void Calculate_Walk_Horizental_Velocity()
    {
        if(isWalk)
        {
            rigid.linearVelocity = new(direct * speed, 0);
            if (GetWalkDestination())
            {
                isWalk = false;
                Idle();
            }

            if (Mathf.Abs(PlayerPosition().x - transform.position.x) < 10f)
            {
                isWalk = false;
                Claw();
            }
        }
    }
    void Calculate_Headbutt_Horizental_Velocity()
    {
        //if (anim.GetBool("isHeadbutt"))
        //{
        //    activeDelay -= Time.deltaTime;
        //    if(activeDelay <= 0)
        //        rigid.velocity = new(direct * maxSpeed, 0);
        //}
    }

    void Calculate_Flying_Horizental_Velocity()
    {
        if (isFlying)
        {
            float xPosition = transform.position.x;
            float velocityY = rigid.linearVelocity.y;
            if (xPosition > maxXposition || xPosition < -maxXposition)
            {
                direct = -direct;
                transform.position = (xPosition > 0) ? new(xPosition - 1f, transform.position.y, 0) : new(xPosition + 1f, transform.position.y, 0);

                int iceXpos = (transform.rotation.y == 1) ? -5 : + 5;
                Vector3 dropPosition = new(niddleDropPosition.transform.position.x + iceXpos, niddleDropPosition.transform.position.y, 0);
                iceNiddles[NiddleIndex()].transform.position = dropPosition;
                iceNiddles[NiddleIndex()].SetActive(true);

                SetRotate();
            }

            if (Mathf.Sign(direct) == Mathf.Sign(rigid.linearVelocity.x) || rigid.linearVelocity.x == 0)
            {
                rigid.linearVelocity = new(rigid.linearVelocity.x + direct * speed * Time.deltaTime, velocityY);
            }
            else
            {
                rigid.linearVelocity = new(-rigid.linearVelocity.x, velocityY);
            }

            if (Mathf.Abs(rigid.linearVelocity.x) >= maxSpeed)
                rigid.linearVelocity = new(maxSpeed * direct, velocityY);
        }
    }

    void SetVerticalVelocity()
    {
        if (takeOff)
        {
            rigid.linearVelocity = isTop ? new(rigid.linearVelocity.x, 0) : new(rigid.linearVelocity.x, speed * 1.5f);
            if(isTop)
            {
                takeOff = false;
            }
        }

        if (isLanding)
        {
            rigid.linearVelocity = isLand ? new(rigid.linearVelocity.x, 0) : new(rigid.linearVelocity.x, -speed * 1.5f);
            if(isLand)
            {
                SetTrigger("isLanding");
                onDamageSprite.sprite = landOnDamagedSprite;
                isLanding = false;
            }
        }
    }

    bool GetWalkDestination()
    {
        bool getPlace = false;
        if (originPosition > moveDestination)
            getPlace = moveDestination >= transform.position.x;
        else if (originPosition < moveDestination)
            getPlace = moveDestination <= transform.position.x;
        return getPlace;
    }

    void Claw()
    {
        anim.SetTrigger("isClaw");
    }

    void HeadButt()
    {
        anim.SetBool("isHeadbutt", true);
    }

    void Breath()
    {
        float waitTime = 1f;

        SetTrigger("readyForBreathing");
        attackMotion = true;

        StartCoroutine(BreathActive(waitTime));
    }

    IEnumerator BreathActive(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetTrigger("isBreathing");
        MasterAudio.PlaySound3DAtTransform("Breath", transform);
    }

    public void FinishBreath()
    {
        attackMotion = false;
        Idle();
    }

    void Flying()
    {
        groundCount = 0;
        takeOff = true;
        SetTrigger("isFlying");
        onDamageSprite.sprite = flyingIdleOnDamagedSprite;
        StartCoroutine(FlyingPattern());
    }

    IEnumerator FlyingPattern()
    {
        float takeOffTime = 1f;
        yield return new WaitForSeconds(takeOffTime);
        FlyingAttack();
    }
    
    void FlyingAttack()
    {

        int randomMax = (flyingCount <= 0) ? 2 : 3;
        int random = Random.Range(0, randomMax);

        if (flyingCount > 2)
            random = 2;

        switch (random)
        {
            case 0:
                SpitTrigger();
                break;
            case 1:
                FlyMove();
                break;
            case 2:
                Landing();
                break;
        }
        flyingCount++;
    }

    void Landing()
    {
        isLanding = true;
        flyingCount = 0;
    }
    public void AnimatorTriggerFlyingIdle()
    {
        SetRotate();
        SetTrigger("isFlyingIdle");
    }

    void SpitTrigger()
    {
        int random = Random.Range(5, 10);
        float delay = 0f;
        for (int i = 0; i <= random; i++)
        {
            StartCoroutine(Spit(delay, (i == random)));
            delay += .4f;
        }
    }

    IEnumerator Spit(float delay, bool isLast)
    {
        yield return new WaitForSeconds(delay);
        direct = (int)Mathf.Sign(PlayerPosition().x - transform.position.x);
        SetRotate();
        SetTrigger("isSpit");
        MasterAudio.PlaySound3DAtTransform("Spit", transform);
        if (isLast)
            StartCoroutine(FlyingPattern());
    }

    public void SpitIceBolt()
    {
        spitIces[SpitIceIndex()].transform.position = spitPosition.transform.position;
        spitIces[SpitIceIndex()].SetDirectionAndRotate(PlayerPosition());
        spitIces[SpitIceIndex()].SetActive(true);
    }

    int SpitIceIndex()
    {
        if (spitIces[currentSpitIndex].gameObject.activeSelf)
            currentSpitIndex++;

        if (currentSpitIndex >= spitIces.Length)
            currentSpitIndex = 0;

        return currentSpitIndex;
    }

    void FlyMove()
    {
        isFlying = true;
        SetTrigger("isFlyingMove");
        onDamageSprite.sprite = flyingOnDamagedSprite;
        StartCoroutine(FlyingMovePattern(thinkTime));
    }

    IEnumerator FlyingMovePattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NiddleDropPattern();
    }

    void NiddleDropPattern()
    {
        float randomTime = 0;

        for(int i = 0; i < iceNiddles.Length; i++)
        {
            randomTime += Random.Range(.05f, .08f);
            StartCoroutine(IceDrop(randomTime, i == iceNiddles.Length - 1));
        }
    }

    IEnumerator IceDrop(float time, bool isLast)
    {
        yield return new WaitForSeconds(time);
        int randomXPosition = Random.Range(-5, 6);
        Vector3 dropPosition = new(niddleDropPosition.transform.position.x + randomXPosition, niddleDropPosition.transform.position.y, 0);
        iceNiddles[NiddleIndex()].transform.position = dropPosition;
        iceNiddles[NiddleIndex()].SetActive(true);

        if(isLast)
        {
            isFlying = false;
            rigid.linearVelocity = Vector2.zero;
            SetTrigger("isFlyingIdle");
            StartCoroutine(FlyingPattern());
        }
    }

    int NiddleIndex()
    {
        if (iceNiddles[currentNiddleIndex].gameObject.activeSelf)
            currentNiddleIndex++;

        if (currentNiddleIndex >= iceNiddles.Length)
            currentNiddleIndex = 0;

        return currentNiddleIndex;
    }

    //TO-DO : �߶��ϸ鼭 �״� ��ǿ� ���� ���� �߰� �ʿ�
    public override void Death()
    {
        rigid.linearVelocity = Vector2.zero;
        if (!isLand)
        {
            isLanding = true;
            SetTrigger("isFalling");
        }
        base.Death();
    }
}
