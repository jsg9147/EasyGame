using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;

    private void Start()
    {   
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.isDeath)
            return;
        int horizontal = 0;
        int vertical = 0;
        if (Input.GetKey(KeySetting.keys[KeyAction.LEFT]))
            horizontal = -1;
        else if (Input.GetKey(KeySetting.keys[KeyAction.RIGTH]))
            horizontal = 1;
        if (Input.GetKey(KeySetting.keys[KeyAction.UP]))
            vertical = 1;
        else if (Input.GetKey(KeySetting.keys[KeyAction.DOWN]))
            vertical = -1;

        //Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 directionalInput = new Vector2(horizontal, vertical);
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]))
            player.OnJumpInputDown();
        if (Input.GetKeyUp(KeySetting.keys[KeyAction.JUMP]))
            player.OnJumpInputUp();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.ATTACK]))
            player.AttackInputDown(directionalInput);

    }
}
