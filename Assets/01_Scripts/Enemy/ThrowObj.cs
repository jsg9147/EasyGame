using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour
{
    public float disapearTime;
    float time;
    private void Start()
    {
        time = disapearTime;
    }
    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().linearVelocity;
        time -= Time.deltaTime;

        if (time <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        time = disapearTime;
    }

    private void OnDisable()
    {
        time = disapearTime;
    }
}
