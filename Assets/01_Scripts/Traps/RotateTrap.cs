using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrap : MonoBehaviour
{
    public float speed;
    public bool right;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }


    void RotateObject()
    {
        if(right)
            transform.Rotate(Vector3.back * Time.deltaTime * speed);
        else
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);

        if (Mathf.Abs(transform.rotation.z) >= 360)
            transform.localEulerAngles = Vector3.zero;
    }
}
