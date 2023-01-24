using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    public static SteamAchievements instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAchievement(string achievement_ID)
    {
        if(!SteamManager.Initialized) { return; }
        // 업적 초기화
        //SteamUserStats.ResetAllStats(true);
        
        SteamUserStats.SetAchievement(achievement_ID);

        SteamUserStats.StoreStats();
    }
}
