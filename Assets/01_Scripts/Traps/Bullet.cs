using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 dir;
    Vector3 startPos;
    float speed;
    float distance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public void SetInfo(Vector3 _startPos, Vector3 _dir, float _speed, float _distance)
    {
        dir = _dir;
        speed = _speed;
        startPos = _startPos;
        distance = _distance;
    }

    void Shoot()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if((transform.position - startPos).magnitude > distance)
        {
            transform.position = startPos;
            gameObject.SetActive(false);
        }    
    }
}
