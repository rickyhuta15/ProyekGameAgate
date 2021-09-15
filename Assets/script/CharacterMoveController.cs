using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController: MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRantio;
    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;
    [Header("Camera")]
    public CameraMoveController gameCamera;
    
    private Rigidbody2D rig;
    private Animator anim;
    private CharacterSoundsController sound;

    private bool isJumping;
    private bool isOnGround;

    private float lastPositionX;

    private void Start()
    {

    rig = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();  
    sound = GetComponent<CharacterSoundsController>(); 

    lastPositionX = transform.position.x;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(isOnGround)
            {
            isJumping = true;
            sound.PlayJump();
            }
        }
       

        anim.SetBool("isOnGround",isOnGround);
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRantio);
        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }
        // buat game overnya
        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

        private void FixedUpdate()
        {
             RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit)
        {
            if (!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround =false;
        }
        Vector2 velocityVector = rig.velocity;

        if(isJumping)
        {
        velocityVector.y += jumpAccel;
        isJumping = false;
        }
        
        velocityVector.x =Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rig.velocity = velocityVector;
        }
        private void GameOver()
        {
            score.FinishScoring();
            gameCamera.enabled = false;
            gameOverScreen.SetActive(true);
            this.enabled = false;
        }
        private void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
        }
 }

    
        