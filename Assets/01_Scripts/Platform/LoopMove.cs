using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMove : MonoBehaviour
{
    public GameObject moveObject;
    GameObject[] moveObjects;
    public float delay;
    public float speed;

    public Vector2 direct;
    float cameraWidthSize;
    float cameraHeightSize;

    int objIndex;
    float currentTime;

    int objectCount = 10;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        PositionReset();
    }

    void Initialize()
    {
        direct = direct.normalized;
        cameraHeightSize = Camera.main.orthographicSize;
        cameraWidthSize = cameraHeightSize * Camera.main.aspect;
        currentTime = 0;
        objIndex = 0;

        ObjectPooling();
    }

    void ObjectPooling()
    {
        moveObjects = new GameObject[objectCount];

        for(int i = 0; i< moveObjects.Length; i++)
        {
            moveObjects[i] = Instantiate(moveObject, transform);
        }
    }

    void Move()
    {
        for (int i = 0; i < moveObjects.Length; i++)
        {
            if(moveObjects[i].activeSelf)
                moveObjects[i].transform.Translate(direct * speed * Time.deltaTime);
        }
    }

    void PositionReset()
    {
        if(currentTime > delay)
        {
            if (!moveObjects[objIndex].activeSelf)
                moveObjects[objIndex].SetActive(true);

            moveObjects[objIndex].transform.localPosition = Vector3.zero;
            objIndex++;

            if (objIndex >= moveObjects.Length)
                objIndex = 0;

            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }
    public void SetDirection(Vector2 direction) => this.direct = direction;
}
