using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherTrap : MonoBehaviour
{
    public GameObject player;
    public Feather featherObj;
    public int featherCount;
    public float minSpeed;
    public float maxSpeed;
    public float delay;

    float nextDelay;
    Feather[] feathers;

    void Start()
    {
        nextDelay = 0f;
        feathers = new Feather[featherCount];
        for(int i =0; i < featherCount; i++)
        {
            Feather feather = Instantiate(featherObj, transform);
            float speed = Random.Range(minSpeed, maxSpeed);

            feather.Init(speed, player, nextDelay);
            feathers[i] = feather;

            feather.gameObject.SetActive(false);

            nextDelay = nextDelay + delay;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < featherCount; i++)
            {
                feathers[i].gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            if (collision.gameObject.tag == "Player")
            {
                for (int i = 0; i < featherCount; i++)
                {
                    feathers[i].gameObject.SetActive(false);
                }
            }
        }
            
    }
}
