using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class GenesisTrap : MonoBehaviour
{
    public Genesis genesisPrefab;
    public float[] xPositions;

    Genesis[] geneses;
    bool isActive;
    private void Start()
    {
        geneses = new Genesis[xPositions.Length];
        GenesisInstantiate();
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            ActiveTrap();
    }

    void GenesisInstantiate()
    {
        for(int i = 0; i < xPositions.Length; i++)
        {
            Genesis genesis = Instantiate(genesisPrefab);
            geneses[i] = genesis;
            genesis.transform.position = new(xPositions[i], transform.position.y + 30f, 0);
            genesis.SetActive(false);
        }
    }

    void ActiveTrap()
    {
        if(!isActive)
        {
            foreach (Genesis genesis in geneses)
            {
                genesis.SetActive(true);
            }
            isActive = true;
        }
    }

}
