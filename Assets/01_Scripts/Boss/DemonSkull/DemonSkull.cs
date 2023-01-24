using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class DemonSkull : Boss
{
    public ObjectManager objectManager;
    public Zombie zombiePrefab;

    public float attackDelay;

    public int summonCount;
    public int genesisCount;

    public List<Transform> spawnPoints;
    public RiseThorn[] riseThorns;

    RigidbodyVelocity[] bulletPrefabs;
    int bulletIndex, genesisIndex;

    Genesis[] genesies;
    float shotSpeed;

    float waveVerticalGap;
    bool waveGapPlus;

    public float bottomYposition;
    public override void Start()
    {
        base.Start();

        bulletIndex = 0;
        genesisIndex = 0;
        waveGapPlus = true;
        ObjectPooling();
        SetIdle();
        //Idle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack")
            OnDamaged(collision.GetComponent<Javelin>());
    }

    public override void OnDamaged(Javelin javelin)
    {
        base.OnDamaged(javelin);
    }

    void ObjectPooling()
    {
        bulletPrefabs = new RigidbodyVelocity[objectManager.bulletPrefabs.Length];
        genesies = new Genesis[objectManager.genesisPrefabs.Length];


        for (int i = 0; i < bulletPrefabs.Length; i++)
        {
            bulletPrefabs[i] = objectManager.bulletPrefabs[i];
        }

        for (int i = 0; i < genesies.Length; i++)
        {
            genesies[i] = objectManager.genesisPrefabs[i];
        }
    }

    //public void Idle()
    //{
    //    anim.SetInteger("skill_index", 0);
    //    anim.SetTrigger("idle_1");

    //    StartCoroutine(AttackPattern());
    //}

    void AnimatorTriggerReset()
    {
        anim.ResetTrigger("isIdle");
        anim.ResetTrigger("isSummon");
        anim.ResetTrigger("BulletsShot");
        anim.ResetTrigger("riseUp");
    }

    public void SetIdle()
    {
        AnimatorTriggerReset();
        SetTrigger("isIdle");

        StartCoroutine(RandomPattern());
    }

    IEnumerator DelayIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetIdle();
    }

    IEnumerator RandomPattern()
    {
        yield return new WaitForSeconds(attackDelay);
        AnimatorTriggerReset();
        int random = Random.Range(0, 4);

        switch(random)
        {
            case 0:
                Summon();
                break;
            case 1:
                SkullBulletsShot();
                break;
            case 2:
                WaveShot();
                break;
            case 3:
                RiseUpAttack();
                break;
        }
    }
    public void SoundPlay(string clipName) => MasterAudio.PlaySound3DAtTransform(clipName, transform);

    public override void Death()
    {
        base.Death();
    }

    void Summon()
    {
        SetTrigger("isSummon");
        
        int[] randomIndex = new int[summonCount];

        for (int i = 0; i < randomIndex.Length; i++)
        {
            randomIndex[i] = Random.Range(0, spawnPoints.Count);
        }

        for (int i = 0; i < summonCount; i++)
        {
            Zombie zombie = Instantiate(zombiePrefab);
            zombie.SetPlayer(gameManager.player);

            zombie.SetPosition(spawnPoints[randomIndex[i]].position);
        }
    }
    void SkullBulletsShot()
    {
        SetTrigger("BulletsShot");

        float delay = 0.5f;
        int fireCount = Random.Range(15, 20);
        for (int i = 0; i < fireCount; i++)
        {
            StartCoroutine(RandomShot(delay));
            delay += .2f;
        }

        StartCoroutine(DelayIdle(delay));
    }

    IEnumerator RandomShot(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bulletIndex >= bulletPrefabs.Length)
            bulletIndex = 0;

        RigidbodyVelocity bullet = bulletPrefabs[bulletIndex];
        bullet.transform.position = transform.position + (Vector3.up * 10f);

        Vector3 direct = PlayerPosition() + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        bullet.SetSpeed(Random.Range(20f, 30f));
        bullet.SetDirectionAndRotate(direct);
        bullet.gameObject.SetActive(true);

        bulletIndex++;
    }
    void WaveShot()
    {
        SetTrigger("BulletsShot");
        float delay = 0.5f;
        waveVerticalGap = 4f;

        int fireCount = 100;

        //shotSpeed = Random.Range(8f, 10f);
        shotSpeed = 10f;
        for (int i = 0; i < fireCount; i++)
        {
            StartCoroutine(DoubleShot(delay));
            delay += .1f;
        }

        DelayIdle(delay);
    }
    IEnumerator DoubleShot(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 direct = Vector3.left;

        if (bulletIndex >= bulletPrefabs.Length)
            bulletIndex = 0;

        int upIndex = bulletIndex;
        int downIndex = (bulletIndex + 1 >= bulletPrefabs.Length) ? 0 : bulletIndex + 1;

        if ((transform.position + (Vector3.down * waveVerticalGap) + (Vector3.up * 4)).y > bottomYposition)
        {
            RigidbodyVelocity bulletDown = bulletPrefabs[downIndex];
            bulletDown.transform.position = transform.position + (Vector3.down * waveVerticalGap) + (Vector3.up * 4);
            bulletDown.SetSpeed(shotSpeed);
            bulletDown.SetDirect(direct);
            bulletDown.gameObject.SetActive(true);
        }

        bulletIndex = bulletIndex + 2;

        RigidbodyVelocity bulletUp = bulletPrefabs[upIndex];
        bulletUp.transform.position = transform.position + (Vector3.up * waveVerticalGap) + (Vector3.up * 4);
        bulletUp.SetSpeed(shotSpeed);
        bulletUp.SetDirect(direct);
        bulletUp.gameObject.SetActive(true);

        if (waveGapPlus)
        {
            waveVerticalGap += 0.3f;
            if (waveVerticalGap > 4.5f)
                waveGapPlus = false;
        }
        else
        {
            waveVerticalGap -= 0.3f;
            if (waveVerticalGap < 2.5f)
                waveGapPlus = true;
        }
    }

    void RiseUpAttack()
    {
        SetTrigger("riseUp");
        float delay = 0;
        for (int i = 0; i < riseThorns.Length; i++)
        {
            StartCoroutine(RiseUpDelay(i, delay));
            delay += 0.1f;
        }

        StartCoroutine(DelayIdle(1f));
    }

    IEnumerator RiseUpDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        riseThorns[index].SetActive(true);
    }

    public void GenesisAttack()
    {
        int[] randomIndex = new int[genesisCount];

        for (int i = 0; i < randomIndex.Length; i++)
        {
            randomIndex[i] = Random.Range(0, spawnPoints.Count);
        }

        float delay = 0f;

        for (int i = 0; i < summonCount; i++)
        {
            StartCoroutine(GenerateGenesis(delay, spawnPoints[randomIndex[i]].position));

            delay += .3f;
        }

        //StartCoroutine(ToIdleAfterAttack(delay));
    }

    IEnumerator GenerateGenesis(float delay, Vector3 position)
    {
        yield return new WaitForSeconds(delay);

        Genesis genesis = genesies[genesisIndex];
        genesis.transform.position = position;
        genesis.gameObject.SetActive(true);
        genesisIndex++;
    }
}
