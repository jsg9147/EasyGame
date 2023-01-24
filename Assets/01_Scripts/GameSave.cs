using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class GameSave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SavePlayerPosition();
        }
    }

    public void SavePlayerPosition()
    {
        Vector2 playerPos = transform.position;

        ES3.Save("PlayerX", playerPos.x);
        ES3.Save("PlayerY", playerPos.y);
        ES3.Save("StageIndex", SceneManager.GetActiveScene().buildIndex);
    }
}
