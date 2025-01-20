using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
public class Genesis : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void EndEvent() => gameObject.SetActive(false);

    public void Attack() => animator.SetTrigger("isAttack");

    public void DelayAttack(float delay)
    {
        StartCoroutine(DelayAttackTrigger(delay));
    }

    IEnumerator DelayAttackTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("isAttack");
        MasterAudio.PlaySound("Laser");
    }
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void PlaySound() => MasterAudio.PlaySound3DAtTransform("Laser", transform);
}
