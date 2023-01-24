using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTranslate : MonoBehaviour
{
    public bool isMove;
    public Vector2 direct;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if(isMove)
            transform.Translate(direct * speed * Time.deltaTime);
    }

    public void StopTime(float time) => Invoke("Stop", 10f);

    void Stop()
    {
        isMove = false;
        gameObject.SetActive(false);
    }    
}
