using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public MoveObject[] movingObjectPrefabs;
    public int objectCount;
    public Vector2 direct;
    void Start()
    {
        ObjectPooling();
        direct = direct.normalized;
    }
    void ObjectPooling()
    {
        for(int i = 0; i < objectCount; i++)
        {
            for(int j = 0; j < movingObjectPrefabs.Length; j++)
            {
                MoveObject movingObj = Instantiate(movingObjectPrefabs[j], transform);
                movingObj.speed = Random.Range(1, 2.5f);
                movingObj.SetDirection(direct);
            }
        }
    }
}
