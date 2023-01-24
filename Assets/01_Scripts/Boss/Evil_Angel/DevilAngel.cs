using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class DevilAngel : Boss
{
    public SpriteRenderer background;
    public Image frontPanel;
    public SkullBullet bulletPrefab;
    public Genesis genesis;
     
    public GameObject tornadoPrefab;
    public GameObject roomTrap;

    public float speed;
    public float thinkTime;
    public float bulletSpeed;
    public float precision;
    Vector3 originPosition;
    bool isStarted;

    float slash, tornado, teleportBackattack, randomAirstrike, bigSmash, fireField;
    SkullBullet[] bullets;
    GameObject[] tornados;
    Genesis[] geneses;
    int bulletIndex;
    int tornadoIndex;

    public Vector3 moveDirect;
    public Vector3 moveDestination;

    bool isMove, isSlash;
    bool isBerserk;

    int lastPattern;
    int genesisIndex;

    public float maxXposition;
    public float minYposition;

    public override void Start()
    {
        base.Start();
        InitialLize();
    }

    private void FixedUpdate()
    {
        StartEffect();
        Move();
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

        if(isDie)
        {
            roomTrap.SetActive(false);
            MasterAudio.PlaySound("BossDeath");
        }
    }
    void BerserkMode()
    {
        if (currentHP < hp/3f && !isBerserk)
        {
            isBerserk = true;
            thinkTime = 2.5f;
            speed += 0.5f;
            spriteRenderer.material.DOColor(new Color(1f, 0.7f, 0.7f, 1f), 2f);
            roomTrap.transform.DOMoveY(2, 3f);
        }
    }

    void InitialLize()
    {
        float startPosition = 15f;
        originPosition = transform.position;
        transform.position = originPosition + (Vector3.up * startPosition);
        isStarted = false;
        isMove = false;
        isBerserk = false;
        objectPooling();

        background.DOColor(new Color(1f, 0.3f, 0.3f), 4f);
    }

    void objectPooling()
    {
        bulletIndex = 0;

        bullets = new SkullBullet[100];

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].SetActive(false);
        }

        tornadoIndex = 0;

        tornados = new GameObject[20];
        for (int i = 0; i < tornados.Length; i++)
        {
            tornados[i] = Instantiate(tornadoPrefab);
            tornados[i].SetActive(false);
        }

        genesisIndex = 0;
        geneses = new Genesis[20];
        for(int i = 0; i < geneses.Length; i++)
        {
            geneses[i] = Instantiate(genesis);
            geneses[i].SetActive(false);
        }
    }

    public void PlaySound(string clipName) => MasterAudio.PlaySound(clipName);
    void SetRotate()
    {
        int direct = FaceDirect();

        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = (direct > 0) ? new(rot.x, 0, rot.z, 0) : new(rot.x, 180, rot.z, 0);
    }
    public void SetIdle()
    {
        SetTrigger("Idle");
        StartCoroutine(RandomPattern());
    }

    IEnumerator DelayIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetIdle();
    }

    IEnumerator RandomPattern()
    {
        yield return new WaitForSeconds(thinkTime);
        SetRotate();
        int randomPatternNumber = RandomPatternIndex();
        SetBool("Idle", false);
        switch (randomPatternNumber)
        {
            case 0:
                DashSlash();
                break;
            case 1:
                Tornado();
                break;
            case 2:
                TeleportBackattack();
                break;
            case 3:
                RandomAirstrike();
                break;
            case 4:
                GenesisPattern();
                break;
            case 5:
                FireField();
                break;
        }
    }


    int RandomPatternIndex()
    {
        int index;
        ImportanceTuning();
        float random = Random.Range(0, 100f);

        float total = PatternImportanceTotal();

        float pattern_0 = slash / total * 100f;
        float pattern_1 = (tornado / total * 100f) + pattern_0;
        float pattern_2 = (teleportBackattack / total * 100f) + pattern_1;
        float pattern_3 = (randomAirstrike / total * 100f) + pattern_2;
        float pattern_4 = (bigSmash / total * 100f) + pattern_3;
        float pattern_5 = (fireField / total * 100f) + pattern_4;

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

        lastPattern = index;

        return index;
    }

    float PatternImportanceTotal()
    {
        float beforeBerserk = slash + tornado + teleportBackattack + randomAirstrike  + bigSmash;
        //float total = isBerserk ?  beforeBerserk + fireField : beforeBerserk;
        float total = beforeBerserk ;

        return total;
    }

    void ImportanceTuning()
    {
        slash = 10;
        tornado = 10f;
        teleportBackattack = 10f;
        randomAirstrike = 10f;
        bigSmash = 10f;
        fireField = 10f;

        if (lastPattern == 4)
            bigSmash = 0;
    }

    void StartEffect()
    {
        if (!isStarted)
        {
            transform.Translate(Vector3.down * speed/3 * Time.deltaTime);
            if (transform.position.y < originPosition.y)
            {
                transform.position = originPosition;
                isStarted = true;
                SetIdle();
            }
        }
    }

    void Move()
    {
        if (!isMove)
            return;

        float moveSpeed = speed;
        transform.Translate(moveDirect * moveSpeed * Time.deltaTime);

        if((transform.position - moveDestination).magnitude < 1f)
        {
            isMove = false;
            if (isSlash)
            {
                SetTrigger("Slash");
                isSlash = false;
            }
        }
    }

    void DashSlash()
    {
        isMove = true;
        isSlash = true;

        SetTrigger("Flying");

        moveDestination = SetDestination();
        moveDirect = moveDestination - transform.position;
        moveDirect.x = Mathf.Abs(moveDirect.x);
        moveDirect = moveDirect.normalized;
    }

    // 게니츠 기술 처럼 나가게
    void Tornado()
    {
        SetTrigger("Tornado");
    }
    public void GenerateTornado()
    {
        tornados[tornadoIndex].transform.position = new(playerPos.x, -61f, 0);
        tornados[tornadoIndex].SetActive(true);
        tornadoIndex++;
    }

    void TeleportBackattack()
    {
        SetTrigger("Teleport");
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        moveDestination = SetDestination();
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        transform.position = moveDestination;
        SetTrigger("TeleportSlash");
        SetRotate();
        SetIdle();
    }
    Vector3 SetDestination()
    {
        Vector3 destination = playerPos + (Vector3.down * 2f);
        
        if(Mathf.Abs(destination.x) > maxXposition)
        {
            destination.x = maxXposition * Mathf.Sign(destination.x);
        }   
        if (destination.y < minYposition)
            destination.y = minYposition;

        return destination;
    }

    void GenesisPattern()
    {
        SetTrigger("Teleport");
        Sequence sequence = DOTween.Sequence().Append(frontPanel.DOColor(new Color(0, 0, 0, 0.8f), 2f))
            .Append(frontPanel.DOColor(new Color(0, 0, 0, 0f), 0.5f))
            .OnComplete(() =>
            {
                GenesisFire();
                transform.position = SetDestination();
            });
    }
    void GenesisFire()
    {
        int fireCount = 3;
        float delay = 0f;
        for(int i = 0; i < fireCount; i ++)
        {
            StartCoroutine(DelayGenesis(delay));
            delay += 0.5f;
        }
        StartCoroutine(DelayIdle(2f));
    }
    IEnumerator DelayGenesis(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (genesisIndex >= geneses.Length)
            genesisIndex = 0;
        geneses[genesisIndex].transform.position = new(Random.Range(-17f, 17f), -50f, 0);
        geneses[genesisIndex].SetActive(true);

        genesisIndex++;
    }

    void RandomAirstrike()
    {
        SetTrigger("Cast_Loop");
        MasterAudio.PlaySound("Ghost");
        float delay = 1.5f;
        int randomCount = Random.Range(15, 20);
        for (int i = 0; i < randomCount; i++)
        {
            StartCoroutine(Set_Bullet_Position(delay));
            delay += 0.5f;
        }
        Invoke("SetIdle", delay);
    }

    IEnumerator Set_Bullet_Position(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bulletIndex >= bullets.Length)
            bulletIndex = 0;

        Vector3 random_Direct = PlayerPosition() + new Vector3(Random.Range(-precision, precision), Random.Range(-precision, precision));

        bullets[bulletIndex].transform.position = RandomPositon_Outside();
        bullets[bulletIndex].SetDirectionAndRotate(random_Direct);
        bullets[bulletIndex].SetSpeed(bulletSpeed);
        bullets[bulletIndex].SetActive(true);
        bulletIndex++;
    }

    Vector3 RandomPositon_Outside()
    {
        bool vertical_Position = Random.value > 0.5f;
        float random_X_Position, random_Y_Position;

        if(vertical_Position)
        {
            random_X_Position = Random.Range(-18.5f, 18.5f);
            random_Y_Position = (Random.value > 0.5f) ? -59.5f : -39.4f;
        }
        else
        {
            random_X_Position = (Random.value > 0.5f) ? - 18.5f : 18.5f;
            random_Y_Position = Random.Range(-59.5f, -39.4f);
        }

        return new Vector3(random_X_Position, random_Y_Position, 0);
    }

    // 발악패턴?
    void FireField()
    {
        SetTrigger("Dead_Loop");
    }
}
