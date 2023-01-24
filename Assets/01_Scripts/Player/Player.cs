using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    public GameObject root;
    public Animator anim;

    [HideInInspector]
    public bool isDeath;

    public Javelin javelinPrefab;
    public Transform hand;
    Javelin[] javelins;
    int javelinIndex;

    public float maxJumpHeight; // 점프 높이
    public float minJumpHeight; // 점프 높이
    public float timeToJumpApex; // 체공시간
    public float maxGravity;
    public float wallSlideTime; // 벽 체공 시간

    float velocityXSmoothing = 0.1f;
    float accelerationTimeGrounded = 0.7f;
    float accelerationTimeAirborne = 0.7f;
    float accelerationTimeSlide = 2f;

    float gravity;

    public Vector2 damagedMove;

    //float accelerationTimeAirborne = .1f;
    //float accelerationTimeGrounded = .05f;

    public float moveSpeed;
    public Vector2 wallJumpClimb; // 벽에 기대고 점프할때 이동 벡터
    public Vector2 wallJumpOff; // 벽타기중 방향키 안누르고 점프시 이동벡터
    public Vector2 wallLeap; // 벽에서 멀어질때 점프하면 이동 벡터(슈퍼마리오 벽점프)

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f; // 벽에 강제로 붙어있는 시간

    float timeTowallUnstick;
    
    float maxJumpVelocity;
    float minJumpVelocity;

    [HideInInspector]
    public bool canMoreJumping;

    Vector3 velocity;
    //float veltocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    bool canWallJump;
    int wallDirX;

    float slideSpeed;

    private float currentTime;
    float attackDelay = 0.1f;

    float timeToWallSlide;
    private void Start()
    {
        Initialize();
        GetStartPoint();
        MasterAudio.PlaySound("Coin");
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        PlayerMove();
        currentTime -= Time.deltaTime; // 공격 딜레이용
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }

        if (collision.transform.tag == "Through" || collision.transform.tag == "Platform")
            SlideSpeedReset();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Through" || collision.transform.tag == "Platform")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
            OnDamaged(collision.transform.position);
        if(collision.gameObject.tag == "CompulsionJump")
        {
            if(Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]))
                velocity.y = maxJumpVelocity * 1.5f;
            else
                velocity.y = maxJumpVelocity;

            MasterAudio.PlaySound3DAtTransform("Spring", transform);
        }
        if (collision.gameObject.tag == "SlideWall")
        {
            controller.collisions.isSlide = false;
            wallSliding = true;
        }

        if (collision.transform.tag == "Through" || collision.transform.tag == "Platform")
            SlideSpeedReset();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SlideWall")
            HandleWallSliding();
    }

    void Initialize()
    {
        isDeath = false;
        controller = GetComponent<Controller2D>();
        javelinIndex = 0;
        timeToWallSlide = wallSlideTime;
        javelins = new Javelin[10];

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        canMoreJumping = true;
        canWallJump = true;

        for (int i = 0; i < 10; i++)
        {
            javelins[i] = Instantiate(javelinPrefab);
            javelins[i].SetPlayer(this);
            javelins[i].gameObject.SetActive(false);
        }
    }

    void GetStartPoint()
    {
        Vector2 startPos = new (ES3.Load("PlayerX", 0f), ES3.Load("PlayerY", 0f));

        transform.position = startPos;
    }

    void SlideSpeedReset()
    {
        slideSpeed = 0f;
        wallSliding = false;
        velocityXSmoothing = 0.1f;
    }

    public void PlayerDeath()
    {
        if(!isDeath)
        {
            MasterAudio.PlaySound("Death");
            int deathCount = ES3.Load("DeathCount", 0);
            DOTween.KillAll();
            deathCount++;
            ES3.Save("DeathCount", deathCount);

            isDeath = true;
        }
        
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;

        if (Input.GetKey(KeySetting.keys[KeyAction.LEFT]))
            root.transform.rotation = new Quaternion(0, 0, 0, 0);
        else if (Input.GetKey(KeySetting.keys[KeyAction.RIGTH]))
            root.transform.rotation = new Quaternion(0, 180, 0, 0);


        //spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        anim.SetBool("isWalking", input.x != 0);
    }

    // Update 에서 사용중
    void PlayerMove()
    {
        if (controller.collisions.isSlide)
            velocity.x = slideSpeed;

        if (isDeath)
            velocity.x = 0f;
        controller.Move(velocity * Time.deltaTime, directionalInput);
        if (controller.collisions.above || controller.collisions.below) // 점프중 or 추락중
        {
            if (controller.collisions.slidingDownMaxSlope) // 미끄러지는중이면
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else
            {
                velocity.y = 0;
                if(controller.collisions.below)
                {
                    canMoreJumping = true;
                }
            }

            //if (controller.collisions.below && anim.GetBool("isJumping"))
            //    anim.SetBool("isJumping", false);
        }
    }

    public void OnJumpInputDown()
    {
        if (wallSliding && canWallJump)
        {
            if (wallDirX == directionalInput.x) // 벽쪽으로 방향키 누르고 점프할때 이동
            {
                velocity.x = -wallDirX * wallJumpClimb.x; // 벽 반대방향으로 x좌표 이동
                velocity.y = wallJumpClimb.y; // 점프량
                canWallJump = false;
                canMoreJumping = false;
            }
            else if (directionalInput.x == 0) // 벽에 붙은채로 방향키 떼고 점프
            {
                //velocity.x = wallDirX * wallJumpOff.x; // 벽 방향으로 정방향 이동점프, 붙음
                velocity.x = -wallDirX * wallJumpOff.x; // 벽 방향으로 정방향 이동점프, 붙음
                velocity.y = wallJumpOff.y;
                canWallJump = false;
                canMoreJumping = false;
            }
            else // 벽에 붙어서 반대 누르고 점프
            {
                velocity.x = -wallDirX * wallLeap.x; // 벽 방향과 반대로 점프
                velocity.y = wallLeap.y;
                canWallJump = false;
                canMoreJumping = false;
            }
            timeToWallSlide = wallSlideTime;
            MasterAudio.PlaySound3DAtTransform("Jump", transform);
        }
        if (controller.collisions.below){ // 바닥밟은건지 확인하는건데 리셋을 자주해서 점프가 씹힘
            if (!controller.collisions.aboveThroughPlatform || directionalInput.y != -1){
                if(controller.collisions.slidingDownMaxSlope){
                    if(directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) // not jumping against max slope
                    {
                        velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                        velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                    }
                }
                else
                {
                    velocity.y = maxJumpVelocity;
                }
            }
            //if (!anim.GetBool("isJumping"))
            //    anim.SetBool("isJumping", true);
            MasterAudio.PlaySound3DAtTransform("Jump", transform);

        }
        else if(canMoreJumping)
        {
            //if (!anim.GetBool("isJumping"))
            //    anim.SetBool("isJumping", true);

            if (controller.collisions.isSlide)
                velocity.x = slideSpeed;

            velocity.y = maxJumpVelocity * 0.7f;

            canMoreJumping = false;
            MasterAudio.PlaySound3DAtTransform("Jump", transform);
        }
    }


    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
            velocity.y = minJumpVelocity;
    }

    public void AttackInputDown(Vector2 direction)
    {
        if (currentTime <= 0)
        {
            ThrowJavelin();

            anim.SetTrigger("isAttack");
            currentTime = attackDelay;
            MasterAudio.PlaySound3DAtTransform("Attack", transform);
        }
    }

    void ThrowJavelin()
    {
        if (javelins[javelinIndex].gameObject.activeSelf)
            return;

        Vector2 direction = root.transform.rotation.y == 0 ? Vector2.left : Vector2.right;

        javelins[javelinIndex].gameObject.SetActive(true);
        javelins[javelinIndex].SetPosition(hand);
        javelins[javelinIndex].SetDirect(direction);
        MasterAudio.PlaySound("Attack");
        javelinIndex++;

        if (javelinIndex >= javelins.Length)
            javelinIndex = 0;
    }

    // 벽타기 기능, 특정 벽에서만 가능하게 만들기
    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        canMoreJumping = true;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) 
        {
            canWallJump = true;
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
                velocity.y = -wallSlideSpeedMax;

            if (timeTowallUnstick > 0)
            {
                //veltocityXSmoothing = 0;
                //velocity.x = 0;
                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    timeTowallUnstick -= Time.deltaTime;
                else
                    timeTowallUnstick = wallStickTime;
            }
        }
        else
        {
            timeTowallUnstick = wallStickTime;
            canWallJump = true;
            wallSliding = false;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;

        if (controller.collisions.isSlide)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX * 2f, ref velocityXSmoothing, accelerationTimeSlide);
            slideSpeed = velocity.x;
        }
        else if(wallSliding)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX * 2f, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            slideSpeed = velocity.x;
        }
        else
            velocity.x = targetVelocityX;

        if (Mathf.Abs(velocity.x) > moveSpeed && !wallSliding)
        {
            velocity.x = moveSpeed * Mathf.Sign(velocity.x);
        }

        velocity.y += gravity * Time.deltaTime; 
        if (velocity.y < -maxGravity)
            velocity.y = -maxGravity;

        if (controller.collisions.below)
            wallSliding = false;

        if (timeToWallSlide > 0)
            timeToWallSlide -= Time.deltaTime;
    }

    void OnDamaged(Vector2 targetPos)
    {
        PlayerDeath();

        gameObject.layer = 11;

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        velocity.x = damagedMove.x * dirc ;
        velocity.y = damagedMove.y ;

        // Animation
        anim.SetTrigger("doDamaged");
    }
}
