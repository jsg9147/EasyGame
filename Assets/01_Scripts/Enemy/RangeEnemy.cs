using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RangeEnemy : Enemy
{
    public SpriteRenderer attack;
    public SpriteRenderer attackWarning;

    public float thickness;
    public float warningTime;
    public float startupTime;
    public float activeTime;
    public float disapearTime;

    public bool isVertical;

    public float attackDelay;

    float nextAttackTime;
    bool attackEnd;

    public override void Start()
    {
        base.Start();
        attackEnd = true;
        nextAttackTime = 1f;

        int direction = spriteRenderer.flipX ? 1 : -1;
        if (!isVertical)
        { 
            attackWarning.transform.localPosition = new Vector3(23 * direction, 0,0);
            attack.transform.localPosition = new Vector3(23* direction, 0,0);
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (!isDie && detected)
            AttackWarning();
    }

    void AttackWarning()
    {
        if (!attackEnd || Time.time < nextAttackTime)
            return;

        Sequence sequence = DOTween.Sequence()
            .Append(attackWarning.DOFade(0.3f, warningTime))
            .AppendCallback(() =>
            {
                attackWarning.DOFade(0, 0.1f);
                RangeAttack();
            });
        attackEnd = false;
    }

    void RangeAttack()
    {
        attack.gameObject.tag = "Enemy";

        if(isVertical)
        {
            Sequence attackSequence = DOTween.Sequence()
           .Append(attack.DOFade(1f, startupTime))
           .Append(attack.transform.DOScaleX(thickness, activeTime))
           .AppendCallback(() =>
           {
               attack.DOFade(0, disapearTime);
               attack.transform.DOScaleY(0, disapearTime);
           }).OnComplete(() => AttackCallback());
        }
        else
        {
            Sequence attackSequence = DOTween.Sequence()
           .Append(attack.DOFade(1, startupTime))
           .Append(attack.transform.DOScaleY(thickness, activeTime))
           .AppendCallback(() =>
           {
               attack.DOFade(0, disapearTime);
               attack.transform.DOScaleY(0, disapearTime);
           }).OnComplete(() => AttackCallback());
        }
        
    }

    void AttackCallback()
    {
        attack.gameObject.tag = "Untagged";
        nextAttackTime = Time.time + attackDelay;
        attackEnd = true;
    }

    public override void Death()
    {
        attackWarning.color = new Color(0, 0, 0, 0);
        attack.color = new Color(0, 0, 0, 0);
        DOTween.Kill(attackWarning);
        DOTween.Kill(attack);
        attack.gameObject.SetActive(false);
        base.Death();
    }
}
