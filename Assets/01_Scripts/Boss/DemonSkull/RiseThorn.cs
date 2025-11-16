using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseThorn : MonoBehaviour
{
    Rigidbody2D rigid;

    public float height;
    public float speed;
    public float descendDelay;

    bool isActive;
    Vector3 originalPosition;

    Vector3 direct;

    void Start()
    {
        originalPosition = transform.localPosition;
        direct = Vector3.up;

        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
            Rise();

        ResetPosition();
    }

    void Rise()
    {
        if (transform.localPosition.y < originalPosition.y + height && direct == Vector3.up)
            rigid.linearVelocity = (direct * speed);
        else
            rigid.linearVelocity = Vector2.zero;

        StartCoroutine(DirectChangeToDescend());
    }

    IEnumerator DirectChangeToDescend()
    {
        yield return new WaitForSeconds(descendDelay);

        direct = Vector3.down;
    }

    void ResetPosition()
    {
        if(direct == Vector3.down)
            rigid.linearVelocity = (direct * speed);

        if (transform.localPosition.y < originalPosition.y)
        {
            isActive = false;
            direct = Vector3.up;
            transform.localPosition = originalPosition;
            rigid.linearVelocity = Vector2.zero;
        }
    }

    public void SetActive(bool active) => this.isActive = active;
}
