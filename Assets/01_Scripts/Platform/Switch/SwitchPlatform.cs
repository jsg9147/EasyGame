using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlatform : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public GameObject onParent;
    public GameObject offParent;
    GameObject[] onPlatfroms;
    GameObject[] offPlatfroms;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        onPlatfroms = new GameObject[onParent.transform.childCount];
        offPlatfroms = new GameObject[offParent.transform.childCount];

        for (int i = 0; i < onParent.transform.childCount; i++)
        {
            onPlatfroms[i] = onParent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < offParent.transform.childCount; i++)
        {
            offPlatfroms[i] = offParent.transform.GetChild(i).gameObject;
        }


        for (int i = 0; i < onPlatfroms.Length; i++)
        {
            onPlatfroms[i].GetComponent<SpriteRenderer>().sprite = offSprite;
            onPlatfroms[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        for (int i = 0; i < offPlatfroms.Length; i++)
        {
            offPlatfroms[i].GetComponent<SpriteRenderer>().sprite = onSprite;
            offPlatfroms[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void ChangeState(bool isOn)
    {
        if(isOn)
        {
            for(int i = 0; i< onPlatfroms.Length; i++)
            {
                onPlatfroms[i].GetComponent<SpriteRenderer>().sprite = onSprite;
                onPlatfroms[i].GetComponent<BoxCollider2D>().enabled = true;
            }

            for (int i = 0; i < offPlatfroms.Length; i++)
            {
                offPlatfroms[i].GetComponent<SpriteRenderer>().sprite = offSprite;
                offPlatfroms[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < onPlatfroms.Length; i++)
            {
                onPlatfroms[i].GetComponent<SpriteRenderer>().sprite = offSprite;
                onPlatfroms[i].GetComponent<BoxCollider2D>().enabled = false;
            }

            for (int i = 0; i < offPlatfroms.Length; i++)
            {
                offPlatfroms[i].GetComponent<SpriteRenderer>().sprite = onSprite;
                offPlatfroms[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
