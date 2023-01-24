using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBoundery : MonoBehaviour
{
    public GameObject trap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trap.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.gameObject.SetActive(false);
    }
}
