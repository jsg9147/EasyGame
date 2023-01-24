using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public RigidbodyVelocity bulletPrefab;
    public Genesis genesisPrefab;

    [HideInInspector]
    public RigidbodyVelocity[] bulletPrefabs;
    [HideInInspector]
    public Genesis[] genesisPrefabs;

    void Start()
    {
        ArrayInitialize();
        ObjectInstantiate();
    }

    void ArrayInitialize()
    {
        bulletPrefabs = new RigidbodyVelocity[200];
        genesisPrefabs = new Genesis[100];
    }

    void ObjectInstantiate()
    {
        if(bulletPrefab)
        {
            for (int i = 0; i < bulletPrefabs.Length; i++)
            {
                bulletPrefabs[i] = Instantiate(bulletPrefab, transform);
                bulletPrefabs[i].SetActive(false);
            }
        }

        if (genesisPrefab)
        {
            for (int i = 0; i < genesisPrefabs.Length; i++)
            {
                genesisPrefabs[i] = Instantiate(genesisPrefab, transform);
                genesisPrefabs[i].SetActive(false);
            }
        }
    }
}
