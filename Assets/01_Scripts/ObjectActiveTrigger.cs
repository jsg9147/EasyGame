using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveTrigger : MonoBehaviour
{
    public GameObject[] objs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            for(int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(true);
            }
        }
    }
}
