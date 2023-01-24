using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ThunderTrap : MonoBehaviour
{
    public Genesis genesisPrefab;
    public GameObject player;
    public float warningTime;
    public float delay;

    float currentTime;
    Genesis genesis;

    Vector3 playerPos
    {
        get { return player.transform.position; }
    }

    void Start()
    {
        genesis = Instantiate(genesisPrefab);
        genesis.SetActive(false);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        ActiveGetnesis();
    }

    void ActiveGetnesis()
    {
        if(currentTime > delay)
        {
            genesis.transform.position = playerPos + (Vector3.up * 20f);
            genesis.SetActive(true);

            currentTime = 0;
        }
    }
}
