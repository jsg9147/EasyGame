using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChase : MonoBehaviour
{
    public GameObject targetObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetObj.transform.position;
    }
}
