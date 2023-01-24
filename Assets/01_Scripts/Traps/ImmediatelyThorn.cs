using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediatelyThorn : MonoBehaviour
{
    public GameObject[] thorn;
    public bool isCross;
    public float delay;
    public float initTime;
    void Start()
    {
        if (isCross)
            Invoke("TrapTrigger", (delay + initTime)/2);
        else
            TrapTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrapTrigger()
    {
        for(int i = 0; i < thorn.Length; i++)
        {
            thorn[i].transform.position = thorn[i].transform.position + Vector3.up;
        }

        Invoke("TrapInit", initTime);
    }

    void TrapInit()
    {
        for (int i = 0; i < thorn.Length; i++)
        {
            thorn[i].transform.position = thorn[i].transform.position + Vector3.down;
        }

        Invoke("TrapTrigger", delay);
    }
}
