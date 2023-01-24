using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNextPlatform : MonoBehaviour
{
    public GameObject nextPlatforms;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        nextPlatforms.SetActive(true);
    }
}
