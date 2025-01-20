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
    float currentTime;

    bool cycle;

    Vector3 currentPosition;

    void Start()
    {
        time = 0;
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
        if (currentTime < 0)
        {
            Vector3 v = currentPosition;

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
            time += Time.deltaTime;
            v.y += maxDistance * Mathf.Cos(time * speed);

            transform.position = v;
        }
        currentTime -= Time.deltaTime;
    }

}
