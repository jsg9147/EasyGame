using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class Minotaur : Boss
{
    public float moveXdestination;
    public float thinkTime;
    public float speed;
    public float dropSpeed;

    public ObjectManager objectManager;


    RigidbodyVelocity[] dropStones;

    int dropStoneIndex;
    bool isMove;
    float dash, wheelwind, slash, stamp, sting;

    bool attackMotion;

    public override void Start()
    {
        base.Start();
        ObjectPooling();
        isMove = false;
        attackMotion = false;
        SetIdle();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            OnDamaged(collision.GetComponent<Javelin>());
        }
    }

    public override void OnDamaged(Javelin javelin)
    {
        base.OnDamaged(javelin);
    }

    void ObjectPooling()
    {
        dropStoneIndex = 0;
        dropStones = new RigidbodyVelocity[objectManager.bulletPrefabs.Length];

        for(int i = 0; i < dropStones.Length; i++)
        {
            dropStones[i] = objectManager.bulletPrefabs[i];
            dropStones[i].SetActive(false);
        }
    }

    void ImportanceTuning()
    {
        dash = 10f;
        wheelwind = 10f;
        slash = 10f;
        stamp = 10;

        if (Mathf.Abs(transform.position.x) >= moveXdestination && (transform.position - playerPos).magnitude < 3f)
        {
            dash = 0f;
            wheelwind = 10f;
            slash = 10f;
            stamp = 0;
        }

        if((transform.position - playerPos).magnitude > 5f)
        {
            slash = 0f;
        }
    }

    void SetRotate()
    {
        int direct = FaceDirect();

        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = (direct > 0) ? new(rot.x, 0, rot.z, 0) : new(rot.x, 180, rot.z, 0);
    }

    void Move()
    {
        if (isDie)
            return;

        if (isMove)
        { 
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if(EndMove())
            {
                isMove = false;
                if(!attackMotion)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Slash();
                    }
                    else
                    {
                        Wheelwind();
                    }
                }
                else
                {
                    attackMotion = false;
                    SetIdle();
                }
            }
        }
    }

    bool EndMove()
    {
        bool isEnd = false; ;

        if (transform.position.x < 0 && -moveXdestination >= transform.position.x)
            isEnd = true;
        if (transform.position.x > 0 && moveXdestination <= transform.position.x)
            isEnd = true;

        return isEnd;
    }
    public void MoveStop() => isMove = false;

    int RandomPatternIndex()
    {
        int index;
        ImportanceTuning();
        float random = Random.Range(0, 100f);

        float total = slash + dash + wheelwind + stamp;

        float pattern_0 = slash / total * 100f;
        float pattern_1 = (dash / total * 100f) + pattern_0;
        float pattern_2 = (wheelwind / total * 100f) + pattern_1;
        float pattern_3 = (stamp / total * 100f) + pattern_2;

        if (random <= pattern_0)
            index = 0;
        else if (pattern_0 < random && random <= pattern_1)
            index = 1;
        else if (pattern_1 < random && random <= pattern_2)
            index = 2;
        else
            index = 3;

        return index;
    }

    IEnumerator PatternThink()
    {
        yield return new WaitForSeconds(thinkTime);
        anim.SetBool("Idle", false);    
        switch (RandomPatternIndex())
        {
            case 0:
                Slash();
                break;
            case 1:
                Dash();
                break;
            case 2:
                Wheelwind();
                break;
            case 3:
                Stamp();
                break;
        }
    }

    public void SetIdle()
    {
        if (isDie)
            return;

        SetRotate();
        anim.SetBool("Idle", true);
        StartCoroutine(PatternThink());
    }

    void Slash()
    {
        float slashDelay = 0.1f;
        anim.SetTrigger("SlashReady");
        StartCoroutine(SlashExecution(slashDelay));
    }

    IEnumerator SlashExecution(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetTrigger("Slash");
    }

    void Wheelwind()
    {
        anim.SetTrigger("Wheelwind");
        attackMotion = true;
        isMove = true;
    }

    void Dash()
    {
        isMove = true;
        SetRotate();
        anim.SetTrigger("Move");
    }

    void Stamp()
    {
        anim.SetTrigger("Stamp");
        float delay = 0.5f;
        for(int i = 0; i < 50; i++)
        {
            StartCoroutine(DropStorn(delay));
            delay += 0.1f;
        }
        StartCoroutine(IdleDelayd(delay));
    }

    IEnumerator IdleDelayd(float delay  )
    {
        yield return new WaitForSeconds(delay);
        SetIdle();
    }
    IEnumerator DropStorn(float delay)
    {
        yield return new WaitForSeconds(delay);
        MasterAudio.PlaySound3DAtTransform("Footstep", transform);
        int randomXpos = Random.Range(-17, 17);

        if (dropStoneIndex >= dropStones.Length)
            dropStoneIndex = 0;

        dropStones[dropStoneIndex].transform.position = new(randomXpos, -92f, 0);
        dropStones[dropStoneIndex].SetActive(true);
        dropStones[dropStoneIndex].SetSpeed(dropSpeed);
        dropStones[dropStoneIndex].SetDirect(Vector3.down);

        dropStoneIndex++;
    }

    void DashSting()
    {
        anim.SetTrigger("Sting");
    }
    public void WeaponSound() => MasterAudio.PlaySound3DAtTransform("WeaponAttack", transform);
}
