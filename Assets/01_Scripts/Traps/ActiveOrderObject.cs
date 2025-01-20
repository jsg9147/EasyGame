using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOrderObject : MonoBehaviour
{
    public GameObject[] orderPlatforms;
    public float delayTime;

    int orderIndex;
    void Start()
    {
        Init();
        PlatformActive();
    }
    void Init()
    {
        orderIndex = 0;
    }

    void PlatformActive()
    {
        for (int i = 0; i < orderPlatforms.Length; i++)
        {
            if (i == orderIndex)
            {
                orderPlatforms[i].SetActive(true);
                if (i + 1 < orderPlatforms.Length)
                {
                    orderPlatforms[i + 1].SetActive(true);
                }
            }
            else
            {
                //orderPlatforms[i].SetActive(false);
                if (i - 1 != orderIndex)
                {
                    orderPlatforms[i].SetActive(false);
                }
            }
        }
        orderIndex++;

        if (orderIndex >= orderPlatforms.Length)
            orderIndex = 0;

        Invoke("PlatformActive", delayTime);
    }
}
