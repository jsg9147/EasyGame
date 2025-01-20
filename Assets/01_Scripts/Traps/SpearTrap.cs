using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    public float startDelay;
    public float delay;
    Animator anim;
    float time;
    void Start()
    {
        time = startDelay;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TimedTrigger();
    }

    void TimedTrigger()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            TriggerActive();
            time = delay;
        }
    }

    void TriggerActive()
    {
        anim.SetBool("isActive", true);
    }

    public void SetIdle()
    {
        anim.SetBool("isActive", false);
    }
}
