using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;
public class Boss : MonoBehaviour
{
    public GameManager gameManager;
    public ClearPortal clearObj;

    public SpriteRenderer onDamageSprite;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public ProgressBarPro progressBar;

    Player player;
    [HideInInspector]
    public Animator anim;

    public float hp;
    [HideInInspector]
    public float currentHP;
    [HideInInspector]
    public Vector3 playerPos
    {
        get
        {
            return PlayerPosition();
        }
    }

    public bool isDie;

    public virtual void Start()
    {
        isDie = false;
        player = gameManager.player;
        currentHP = hp;
        progressBar.SetValue(currentHP, hp);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector3 PlayerPosition() => player.transform.position;
    public float DistanceToPlayer() => player.transform.position.x - transform.position.x;
    public int FaceDirect() => (int)Mathf.Sign(PlayerPosition().x - transform.position.x);

    public virtual void OnDamaged(Javelin javelin)
    {
        currentHP = currentHP - javelin.damage;
        onDamageSprite.color = new (1, 1, 1, 1);

        SetProgressBar();
        if (currentHP <= 0)
            Death();

        StartCoroutine(DamagedSpriteAlpha());
    }

    IEnumerator DamagedSpriteAlpha()
    {
        yield return new WaitForSeconds(.05f);
        onDamageSprite.color = new(1, 1, 1, 0);
    }

    public virtual void Death()
    {
        if(spriteRenderer)
        {
            spriteRenderer.DOFade(0f, 3f);
        }
        MasterAudio.StopAllPlaylists();
        StopAllCoroutines();
        MasterAudio.PlaySound("Clear");
        gameManager.clearEffect.SetActive(true);
        SetTrigger("isDeath");
        isDie = true;
        GetComponent<Collider2D>().isTrigger = true;

        StartCoroutine(StageClear());
    }

    public void SetProgressBar()
    {
        progressBar.SetValue(currentHP, hp);
    }

    public void SetTrigger(string triggerName) => anim.SetTrigger(triggerName);
    public void SetBool(string boolName, bool isActive) => anim.SetBool(boolName, isActive);

    IEnumerator StageClear(float delay = 1f)
    {
        int stageIndex = ES3.Load("StageIndex", 1);
        string achievement_ID = stageIndex.ToString() + "_STAGE_CLEAR";
        SteamAchievements.instance.SetAchievement(achievement_ID);

        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        clearObj.gameObject.SetActive(true);
    }
}
