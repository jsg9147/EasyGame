using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject boss;
    public GameObject prograssBar;

    private void Start()
    {
        boss.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            boss.SetActive(true);
            prograssBar.SetActive(true);
            gameManager.EnterBossRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            boss.SetActive(false);
            prograssBar.SetActive(false);
        }
    }
}
