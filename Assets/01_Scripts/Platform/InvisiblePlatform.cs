using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    public float delay;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();

        Invoke("PlayAnimaion", delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAnimaion()
    {
        anim.Play("Invisible platform", -1, 0f);

        Invoke("PlayAnimaion", delay);
    }
}
