using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceNiddle : MonoBehaviour
{
    public float startTime;
    public float delayTime;

    Animator anim;

    float currentTime;
    void Start()
    {
        anim = GetComponent<Animator>();
        currentTime = 0;
        AnimatorRepetitive();
    }
    private void Update()
    {
        AnimatorRepetitive();
    }

    void AnimatorRepetitive()
    {
        if(currentTime >= delayTime)
        {
            anim.SetBool("isActive", true);
            currentTime = 0;
        }

        currentTime += Time.deltaTime;
    }

    public void ActiveSelf()
    {
        anim.SetBool("isActive", !anim.GetBool("isActive"));
    }
}
