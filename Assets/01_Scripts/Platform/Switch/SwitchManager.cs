using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SwitchManager : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;
    public SwitchPlatform switchPlatforms;
    public GameObject switchParent;

    SwitchControl[] switchControls;

    bool isOn;

    private void Start()
    {
        switchControls = new SwitchControl[switchParent.transform.childCount];
        for(int i = 0; i < switchControls.Length; i ++)
        {
            switchControls[i] = switchParent.transform.GetChild(i).GetComponent<SwitchControl>();
        }
        isOn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack")
            SwitchOperate();
    }

    public void SwitchOperate()
    {
        isOn = !isOn;

        for (int i = 0; i < switchControls.Length; i++)
        {
            switchControls[i].switchSprite.sprite = isOn ? onSprite : offSprite;
        }
        switchPlatforms.ChangeState(isOn);
    }
}
