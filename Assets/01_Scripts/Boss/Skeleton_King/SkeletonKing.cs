using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class SkeletonKing : Boss
{
    public Transform objectParent;
    public GameObject[] wispPositions;
    public Vector3 centerTopPosition;

    public float productDelay;
    public float meleeArange;
    public float walkSpeed;
    public float slashSpeed;
    public float missileSpeed;
    public float explotionSpacing;
    public float thinkTime;

    public RigidbodyVelocity slashPrefab;
    public RigidbodyVelocity fireMissilePrefab;
    public GameObject explosionPrefab;
    public GameObject fire_Obj;

    float walk, sting, slash, explosion, flying_Fire, wispFire;

    RigidbodyVelocity[] throwSlashs;
    RigidbodyVelocity[] fireMissles;

    Rigidbody2D rigid;
    GameObject[] explosions;
    GameObject[] explosions_2;
    int throwSlashIndex;
    int fireMissleIndex;

    bool isFlying;
    bool isAttacking;
    bool isBerserk;
    bool isLanding;

    bool isDash;

    Vector3 movePoint;
    float dashTime;

    int fireballCount = 2;

    public override void Start()
    {
        base.Start();
        isBerserk = false;
        isAttacking = false;
        isLanding = false;

        isDash = false;
        rigid = GetComponent<Rigidbody2D>();
        ObjectPooling();
        StartProduct();
    }

    private void Update()
    {
        MoveCenterTop();
        Landing();
        HorizentalMove();
        DashMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            OnDamaged(collision.transform.GetComponent<Javelin>());
        }
    }
    public override void OnDamaged(Javelin javelin)
    {
        base.OnDamaged(javelin);

        BerserkMode();
    }

    bool GetBool(string boolName) => anim.GetBool(boolName);

    void BerserkMode()
    {
        if (currentHP < (hp / 2f) && !isBerserk)
        {
            isBerserk = true;

            spriteRenderer.color = new(1f, 0.7f, 0.7f);

            fireballCount = fireballCount * 2;
            slashSpeed = slashSpeed * 1.5f;
            missileSpeed = missileSpeed * 1.5f;
            thinkTime = thinkTime * 0.8f;
        }
    }

    public override void Death()
    {
        base.Death();
    }

    void ObjectPooling()
    {
        throwSlashIndex = 0;
        fireMissleIndex = 0;
        throwSlashs = new RigidbodyVelocity[10];
        explosions = new GameObject[10];
        explosions_2 = new GameObject[10];
        fireMissles = new RigidbodyVelocity[1000];

        for (int i = 0; i < throwSlashs.Length; i++)
        {
            throwSlashs[i] = Instantiate(slashPrefab, objectParent);
            throwSlashs[i].SetActive(false);
        }

        for (int i = 0; i < explosions.Length; i++)
        {
            explosions[i] = Instantiate(explosionPrefab, transform);
            explosions[i].transform.localPosition = new Vector3(explotionSpacing * (i + 1), 0.12f, 0);
            explosions[i].SetActive(false);
        }

        for (int i = 0; i < explosions_2.Length; i++)
        {
            explosions_2[i] = Instantiate(explosionPrefab, transform);
            explosions_2[i].transform.localPosition = new Vector3(explotionSpacing * (i + 1) - (explotionSpacing/2), 0.12f, 0);
            explosions_2[i].SetActive(false);
        }

        for (int i = 0; i < fireMissles.Length; i++)
        {
            fireMissles[i] = Instantiate(fireMissilePrefab, objectParent);
            fireMissles[i].gameObject.SetActive(false);
        }
    }

    void SetRotate()
    {
        int direct = FaceDirect();

        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = (direct > 0) ? new(rot.x, 0, rot.z, 0) : new(rot.x, 180, rot.z, 0);
    }

    // 상황에 따른 배율 조정
    void RandomPattern()
    {
        int randomPatternNumber = RandomPatternIndex();

        SetBool("Idle", false);
        switch (randomPatternNumber)
        {
            case 0:
                Sting();
                break;
            case 1:
                Slash();
                break;
            case 2:
                Walk();
                break;
            case 3:
                Explosion();
                break;
            case 4:
                Flying_FireBall();
                break;
            case 5:
                WispFire();
                break;
        }
    }

    public void SetIdle()
    {
        if (GetBool("Idle"))
            return;

        isAttacking = false;
        SetBool("Idle", true);
        SetRotate();
        StartCoroutine(NextActive());
    }
    IEnumerator DelayIdle(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetIdle();
    }

    IEnumerator NextActive()
    {
        yield return new WaitForSeconds(thinkTime);
        RandomPattern();
    }

    void StartProduct()
    {
        StartCoroutine(SetStartPosition());
    }

    IEnumerator SetStartPosition()
    {
        yield return new WaitForSeconds(productDelay);

        //TODO : 화면 어두워지는 효과 넣기

        transform.position = new Vector3(transform.position.x + 14f, -57.5f, 0f);
        GetComponent<BoxCollider2D>().enabled = true;
        SetBool("isStart", true);
        SetIdle();
    }

    void ImportanceTuning()
    {
        sting = 10f;
        walk = 10f;
        slash = 10;
        explosion = 10f;
        flying_Fire = 4f;
        wispFire = 5f;
    }

    float PatternImportanceTotal()
    {
        float beforeBerserk = sting + walk + slash + explosion + flying_Fire;
        float total = isBerserk ? beforeBerserk + wispFire : beforeBerserk;
        return total;
    }

    void Sting()
    {
        SetTrigger("Sting");
        MasterAudio.PlaySound("Sting");
    }

    void Slash()
    {
        SetTrigger("Slash");
        MasterAudio.PlaySound("Sting");
    }
    void DashMove()
    {
        if (isDash)
        {
            dashTime += Time.deltaTime;
            transform.Translate(Time.deltaTime * walkSpeed * 2.5f, 0,0);

            if (dashTime > 0.4f)
            {
                isDash = false;
                SetIdle();
            }
        }
    }
    public void DashTrue()
    {
        dashTime = 0;
        isDash = true;
    }

    void Explosion()
    {
        SetTrigger("Explosion");
    }

    void Walk()
    {
        SetBool("Walk", true);
        movePoint = new(Random.Range(transform.position.x, PlayerPosition().x), -57.5f, 0);
    }

    void WispFire()
    {
        SetTrigger("Wisp");
        MasterAudio.PlaySound("BigFire");
    }

    public void ActiveWisp()
    {
        if (isAttacking)
            return;

        SetBool("Idle", false);

        for (int i = 0; i < wispPositions.Length; i++)
        {
            wispPositions[i].SetActive(true);
        }

        int minCount = 15;
        int maxCount = 30;

        int randomCount = Random.Range(minCount, maxCount);

        float delay = 2f;

        for (int i = 0; i < randomCount; i++)
        {
            delay += 0.3f;
            StartCoroutine(CoveringWispFire(delay, i == randomCount - 1));
        }
        isAttacking = true;
    }

    IEnumerator CoveringWispFire(float wispDelay, bool isLast)
    {
        yield return new WaitForSeconds(wispDelay);

        // 밑에서 가져옴
        for (int i = 0; i < wispPositions.Length; i++)
        {
            if (fireMissleIndex >= fireMissles.Length)
                fireMissleIndex = 0;

            float wispXPosition = wispPositions[i].transform.position.x;
            float randomXposion = Random.Range(wispXPosition - 25f, wispXPosition + 25f);
            Vector2 direct = new Vector2(randomXposion, transform.position.y).normalized;

            fireMissles[fireMissleIndex].transform.position = wispPositions[i].transform.position;
            fireMissles[fireMissleIndex].SetDirect(direct);
            fireMissles[fireMissleIndex].SetSpeed(missileSpeed);
            fireMissles[fireMissleIndex].SetActive(true);
            fireMissleIndex++;
        }

        if (isLast)
        {
            for (int i = 0; i < wispPositions.Length; i++)
            {
                wispPositions[i].SetActive(false);
            }

            SetIdle();
        }
    }

    void HorizentalMove()
    {
        if(GetBool("Walk"))
        {
            Vector3 direct = movePoint - transform.position;
            rigid.velocity = new(direct.normalized.x * walkSpeed, 0);

            if(direct.magnitude < 1f)
            {
                rigid.velocity = Vector2.zero;
                SetBool("Walk", false);
                SetIdle();
            }
        }
    }

    void Flying_FireBall()
    {
        isFlying = true;
    }

    public void ExplosionEffect()
    {
        float delay = 0;

        int random = Random.Range(0, 2);
        for (int i = 0; i < explosions.Length; i++)
        {
            if(random == 0)
                StartCoroutine(ActiveExplosion(i, delay));
            else
                StartCoroutine(ActiveExplosion_2(i, delay));
            delay += 0.4f;
        }

        if (isBerserk)
            delay -= 1.2f;

        StartCoroutine(DelayIdle(delay));
    }

    IEnumerator ActiveExplosion(int index , float delay)
    {
        yield return new WaitForSeconds(delay);

        explosions[index].SetActive(true);
    }

    IEnumerator ActiveExplosion_2(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        explosions_2[index].SetActive(true);
    }

    public void ThrowSlash()
    {
        if (throwSlashIndex >= throwSlashs.Length)
            throwSlashIndex = 0;

        throwSlashs[throwSlashIndex].SetActive(true);
        throwSlashs[throwSlashIndex].SetDirect(Vector3.right * FaceDirect());
        throwSlashs[throwSlashIndex].SetSpeed(slashSpeed);
        throwSlashs[throwSlashIndex].transform.position = transform.position + (new Vector3(0.55f, 0.05f, 0) * FaceDirect());
        throwSlashIndex++;
    }
    void MoveCenterTop()
    {
        if (isFlying)
        {
            SetBool("Flying", true);
            SetBool("Idle", false);
            transform.position = Vector3.MoveTowards(transform.position, centerTopPosition, 0.15f);
            if (transform.position == centerTopPosition)
            {
                FlyingMissile();
                SetBool("Fireball_Start", true);
            }
        }
    }

    void Landing()
    {
        if (!GetBool("isStart"))
            return;

        if (isLanding)
        {
            Vector3 landingPosition = new Vector3(transform.position.x, -57.3f, 0);
            transform.position = Vector3.MoveTowards(transform.position, landingPosition, 0.15f);
            if (transform.position == landingPosition && !anim.GetBool("Idle"))
            {
                isLanding = false;
                SetIdle();
            }
        }
    }

    void FlyingMissile()
    {
        if (isAttacking)
            return;

        SetTrigger("Cast");
        MasterAudio.PlaySound("BigFire");
        int minCount = 30;
        int maxCount = 50;
        int randomCount = Random.Range(minCount, maxCount);

        float delay = 0f;

        for (int i = 0; i< randomCount; i++)
        {
            delay += 0.3f;
            StartCoroutine(RandomMissile(delay, i == randomCount -1));
        }
        isAttacking = true;
    }

    IEnumerator RandomMissile(float delay, bool isLast)
    {
        yield return new WaitForSeconds(delay);
        

        for(int i =0; i< 3; i++)
        {
            if (fireMissleIndex >= fireMissles.Length)
                fireMissleIndex = 0;

            float randomXposion = Random.Range(-60f, 60f);
            Vector2 direct = new Vector2(randomXposion, transform.position.y).normalized;

            fireMissles[fireMissleIndex].transform.position = fire_Obj.transform.position;
            fireMissles[fireMissleIndex].SetDirect(direct);
            fireMissles[fireMissleIndex].SetSpeed(missileSpeed);
            fireMissles[fireMissleIndex].SetActive(true);
            fireMissleIndex++;
        }

        if(isLast)
        {
            isFlying = false;
            isLanding = true;
            SetBool("Fireball_Start", false); 
            SetBool("Flying", false);
        }    
    }

    int RandomPatternIndex()
    {
        int index;
        ImportanceTuning();

        float random = Random.Range(0, 100f);

        float total = PatternImportanceTotal();

        float pattern_0 = sting / total * 100f;
        float pattern_1 = (walk / total * 100f) + pattern_0;
        float pattern_2 = (slash / total * 100f) + pattern_1;
        float pattern_3 = (explosion / total * 100f) + pattern_2;
        float pattern_4 = (flying_Fire / total * 100f) + pattern_3;
        float pattern_5 = (wispFire / total * 100f) + pattern_4;

        if (random <= pattern_0)
            index = 0;
        else if (pattern_0 < random && random <= pattern_1)
            index = 1;
        else if (pattern_1 < random && random <= pattern_2)
            index = 2;
        else if (pattern_2 < random && random <= pattern_3)
            index = 3;
        else if (pattern_3 < random && random <= pattern_4)
            index = 4;
        else 
            index = 5;

        return index;
    }
}
