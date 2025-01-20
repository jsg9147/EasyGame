using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMove : MonoBehaviour
{
    public GameObject rotateObjectPrefab;
    public int objectCount;
    public float objSpeed = 100f; //원운동 속도
    public float circleR = 1f; //반지름

    public bool objectRotate;

    float deg; //각도
    GameObject[] rotateObjects;
    // Start is called before the first frame update
    void Start()
    {
        rotateObjects = new GameObject[objectCount];

        for (int i = 0; i <objectCount; i++)
        {
            rotateObjects[i] = Instantiate(rotateObjectPrefab, transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    private void OnDisable()
    {
        objSpeed = 100f;
    }

    void Rotate()
    {
        deg += Time.deltaTime * objSpeed;

        if (objSpeed < 1000f)
            objSpeed += 0.1f;

        if (deg < 360)
        {
            for(int i = 0; i < rotateObjects.Length; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / rotateObjects.Length)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);

                rotateObjects[i].transform.position = transform.position + new Vector3(x, y);
                if(objectRotate)
                    rotateObjects[i].transform.rotation = Quaternion.Euler(0, 0, deg * -1); //가운데를 바라보게 각도 조절
            }
        }
        else
        {
            deg = 0;
        }
    }
}
