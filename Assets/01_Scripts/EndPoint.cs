using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EndPoint : MonoBehaviour
{
    public GameManager gameManager;
    public bool existBoss = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            EnterBossRoom();
    }

    void EnterBossRoom()
    {
        if (existBoss)
            gameManager.EnterBossRoom();
        else
            StartCoroutine(StageClear());
    }

    IEnumerator StageClear()
    {
        int stageIndex = ES3.Load("StageIndex", 1);
        string achievement_ID = stageIndex.ToString() + "_STAGE_CLEAR";
        SteamAchievements.instance.SetAchievement(achievement_ID);

        yield return new WaitForSeconds(2f);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        ES3.Save("StageIndex", sceneIndex + 1);
        ES3.Save("PlayerX", 0f);
        ES3.Save("PlayerY", 0f);
        SceneManager.LoadScene(sceneIndex + 1);
    }
}
