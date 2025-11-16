using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public float speed;

    //TODO : 이후 보스몬스터가 소환할때 함수로 할당하는 형식
    public Player player;

    bool moveStart;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        moveStart = false;
    }

    void Update()
    {
        MoveToPlayer();
        SetRotate();
    }

    public void SetPlayer(Player _player) => this.player = _player;

    public void SetPosition(Vector3 spawnPosition) => transform.position = spawnPosition;
    public void MoveStart()
    {
        moveStart = true;
        DarkTonic.MasterAudio.MasterAudio.PlaySound("ZombieSpawn");
    }



        void MoveToPlayer()
    {
        if(moveStart)
        {
            Vector2 playerDir = player.transform.position - transform.position;

            rigid.linearVelocity = new(Mathf.Sign(playerDir.x) * speed, 0);
        }
    }

    void SetRotate()
    {
        transform.rotation = rigid.linearVelocity.x >= 0 ? new Quaternion(0, 0, 0, 0) : new Quaternion(0, 180, 0, 0);
    }

    void SpawnAnimation()
    {
        anim.SetTrigger("isSpawn");
    }

    public void SoundPlay(string clipString) => DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(clipString, transform);
}
