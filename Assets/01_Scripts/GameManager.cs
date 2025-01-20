using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class GameManager : MonoBehaviour
{
    public GameOption gameOption;
    public GameObject clearEffect;

    public Player player;
    public TMP_Text deathCount_Text;
    public TMP_Text resetCount_Text;

    public Transform bossRoom;
    public Transform bossRoomStartPoint;
    CameraFollow cameraFollow;

    public GameObject deathScreen;
    public bool bossBattleStart;

    int deathCount, resetCount;

    private void Start()
    {
        deathCount = ES3.Load("DeathCount", 0);
        resetCount = ES3.Load("ResetCount", 0);

        deathCount_Text.text = deathCount.ToString();
        resetCount_Text.text = resetCount.ToString();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        deathScreen.SetActive(false);
        clearEffect.SetActive(false);
    }

    private void Update()
    {
        OptionWindow();

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.RESET]))
        {
            resetCount++;
            DOTween.KillAll();
            ES3.Save("ResetCount", resetCount);
            Restart();
        }

        if (player.isDeath)
            OnDeathScreen();
    }

    void OptionWindow()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameOption.ActiveMenuWindow();
        }
    }

    public void SavePlayerPosition()
    {
        Vector2 playerPos = player.transform.position;

        ES3.Save("PlayerX", playerPos.x);
        ES3.Save("PlayerY", playerPos.y);
        ES3.Save("StageIndex", SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnDeathScreen()
    {
        if (!deathScreen.activeSelf)
        {
            deathScreen.SetActive(true);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnterBossRoom()
    {
        Camera.main.transform.position = bossRoom.position + (Vector3.back * 10);
        cameraFollow.HoldCameraPosition();
        player.transform.position = bossRoomStartPoint.position;

        ES3.Save("PlayerX", bossRoomStartPoint.position.x);
        ES3.Save("PlayerY", bossRoomStartPoint.position.y);
        ES3.Save("StageIndex", SceneManager.GetActiveScene().buildIndex);

        MasterAudio.StartPlaylist("BossBGM");
        MasterAudio.PlaySound("BossStart");
    }
}
