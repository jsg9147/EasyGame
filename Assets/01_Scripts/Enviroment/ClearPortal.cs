using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;

public class ClearPortal : MonoBehaviour
{
    public GameObject player;
    public GameObject endingCredit;
    BoxCollider2D boxCollider;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.SetActive(false);
            animator.SetTrigger("Close");
            EnterPlayer();
        }
    }


    void EnterPlayer()
    {   
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex < 6)
        {
            ES3.Save("StageIndex", sceneIndex + 1);
            ES3.Save("PlayerX", 0f);
            ES3.Save("PlayerY", 0);
            StartCoroutine(NextStage(sceneIndex + 1));
        }
        else
        {
            ES3.Save("StageIndex", 1);
            ES3.Save("PlayerX", 0f);
            ES3.Save("PlayerY", 0f);
            StartCoroutine(EndingCredit());
        }
    }

    public void OpenSound() => MasterAudio.PlaySound("PortalOpen");
    public void CloseSound() => MasterAudio.PlaySound("PortalClose");

    IEnumerator EndingCredit()
    {
        yield return new WaitForSeconds(1.5f);

        endingCredit.SetActive(true);

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    IEnumerator NextStage(int sceneIndex)
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneIndex);
    }
}
