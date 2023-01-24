using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTriggerTrap : MonoBehaviour
{
    public float startDelay;
    public float delay;
    public float speed;
    public float maxDistance;

    float time;
    float currentTime = 0f;

    bool cycle;

    Vector3 currentPosition;

    void Start()
    {
        currentPosition = transform.position;
        currentTime = startDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(currentTime < 0)
        {
            Vector3 v = currentPosition;

            v.y += maxDistance * Mathf.Cos(time * speed);
            time += Time.deltaTime;

            transform.position = v;

            if (Mathf.Cos(time * speed) <= -0.99f && cycle)
            {
                currentTime = delay;
                cycle = false;
            }

            if (Mathf.Cos(time * speed) >= 0.99f)
            {
                cycle = true;
                time = 0;
            }
        }
        currentTime -= Time.deltaTime;
    }

}
