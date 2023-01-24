using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventReceiever : MonoBehaviour
{
    public Animator monsterAnim;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Attack() => anim.SetBool("isAttack", true);
    public void Idle()
    {
        anim.SetBool("isAttack", false);
        monsterAnim.SetTrigger("idle");
    }
}
