using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody gameObject for jump physics
    public Rigidbody2D playerRb;

    // Horizontal movement variables
    public int playerSpeed = 10;
    public float moveX;

    // Horizontal boundary variables
    public int leftBound = -10;
    public int rightBound = 7;

    // Jump variables
    public float jumpForce;
    public float gravityModifier;

    // Ground check boolean
    private bool isOnGround = false;

    // Air check boolean
    private bool isJumping = false;

    // Static ground check boolean
    private bool touchGrass = false;

    public bool susMode = false;

    // Sprite change gameObjects
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize rigidbody to player's RigidBody2D
        playerRb = GetComponent<Rigidbody2D>();

        // Apply GravityModifier
        Physics2D.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement is handled on separate void function PlayerMove()
        PlayerMove();
    }

    void PlayerMove()
    {
        if (susMode == false)
        {
            // Detect horizontal input
            moveX = Input.GetAxis("Horizontal");

            // Move player left or right depending on moveX and playerSpeed
            transform.Translate(Vector2.right * moveX * Time.deltaTime * playerSpeed);

            // Jump with spacebar
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;

                if (touchGrass)
                {
                    // If player jumps off the floor, they aren't on the floor until they touch floor again
                    touchGrass = false;
                }
            }
        }

        // Press and hold down to become SUS. Being SUS will change the way square colliders interact with the player on the ground and in the air, but take away all other controls.
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
            susMode = true;
            }

    }

    // Move player with box if they are standing on it
    // Boxes have 90-degree platform effectors that let the player pass through their sides, so if they are touching the top of a box, they will stand on it
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Background"))
        {
            if (isJumping)
            {
                // Player has landed from a jump
                isJumping = false;
            }
           
            // Background is ground
            isOnGround = true;

            // Player is touching static ground
            touchGrass = true;
        }
        // Square is box in Spawn Manager
        if (col.gameObject.name.Equals("Square"))
        {
            // Player has landed from a jump
            isJumping = false;

            // square is ground
            isOnGround = true;

            if (touchGrass == false)
            {
                // Player has landed from a jump
                isJumping = false;

                // Player parents the square, and is dragged along with it
                transform.parent = col.transform;
            }
        }

            if (col.gameObject.name.Equals("Coin"))
        {
            Destroy (col.gameObject);
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Background") && isJumping)
        {
            // Background is ground
            isOnGround = false;
        }
        // Square = box in Spawn Manager
        if (col.gameObject.name.Equals("Square"))
        {
            if (isJumping)
            {
                // square is ground
                isOnGround = false;
            }
            

            if (touchGrass == false)
            {
                // Player unparents the square
                transform.parent = null;
            }

            
        }
    }
}
        
