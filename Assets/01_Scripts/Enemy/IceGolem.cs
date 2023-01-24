using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGolem : Enemy
{
    public GameObject lager;
    public float delay = 3f;

    float currentTime;
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        Attack();
    }

    public override void OnDamaged()
    {
        base.OnDamaged();
    }

    public override void Death()
    {
        anim.SetTrigger("idle");
        base.Death();
        DarkTonic.MasterAudio.MasterAudio.PlaySound("IceGolemDeath");
    }

    void Attack()
    {
        if (detected && currentTime > delay)
        {
            anim.SetTrigger("isAttack");
            currentTime = 0;
        }

        currentTime += Time.deltaTime;
    }

    public void LagerOn()
    {
        anim.SetTrigger("idle");
    }
}
