using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectTrap : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        animator.SetTrigger("isDetected");
    }

    public void AttackTrigger()
    {
        animator.SetTrigger("isAttack");
    }

    public void SoundPlay() => DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform("Laser", transform);
}
