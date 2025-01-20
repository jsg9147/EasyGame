using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Feather : MonoBehaviour
{
    public GameObject player;

    float delay;
    float speed;

    float cameraWidthSize;
    float cameraHeightSize;

    float currentTime;

    void Start()
    {
        cameraHeightSize = Camera.main.orthographicSize;
        cameraWidthSize = cameraHeightSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        FlyFeather();
        currentTime += Time.deltaTime;
    }

    public void Init(float _speed, GameObject _player, float _delay)
    {
        speed = _speed;
        player = _player;
        delay = _delay;
    }

    void FlyFeather()
    {
        if(currentTime > delay)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

            if (transform.position.x < player.transform.position.x - (Camera.main.orthographicSize * 2))
            {
                SetPosition();
                currentTime = 0;
            }
        }
        else
            SetPosition();
    }

    void SetPosition()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        float xPosition = cameraPosition.x + cameraWidthSize + 10f;
        float randomYposition = Random.Range(playerPosition.y - 10f, playerPosition.y + 10f);

        transform.position = new Vector3(xPosition, randomYposition, 0);
    }
}
