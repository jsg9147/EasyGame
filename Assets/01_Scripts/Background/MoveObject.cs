using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public bool heightHold;
    public float speed;

    public Vector2 direct;
    float cameraWidthSize;
    float cameraHeightSize;
    void Start()
    {
        direct = direct.normalized;
        cameraHeightSize = Camera.main.orthographicSize;
        cameraWidthSize = cameraHeightSize * Camera.main.aspect;

        if(!heightHold)
            SetStartPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        PositionReset();
    }

    void Move()
    {
        transform.Translate(direct * speed * Time.deltaTime);
    }

    void PositionReset()
    {
        if(transform.localPosition.x < - cameraWidthSize * 1.5f)
        {
            float randomHeight = heightHold ? transform.localPosition.y : Random.Range(-cameraHeightSize, cameraHeightSize);

            transform.localPosition = new Vector3(cameraWidthSize * 1.5f, randomHeight, 10);

            if (!heightHold)
                speed = Random.Range(1f, 2f);
        }
    }

    void SetStartPosition()
    {
        float randomXposiont = Random.Range(-cameraWidthSize * 1.3f, cameraWidthSize * 1.3f);
        float randomYposiont = Random.Range(-cameraHeightSize, cameraHeightSize);
        transform.localPosition = new Vector3(randomXposiont, randomYposiont, 10);
    }

    public void SetDirection(Vector2 direction) => this.direct = direction;
}
