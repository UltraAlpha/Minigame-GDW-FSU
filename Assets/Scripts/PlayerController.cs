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
    public float rightBound = 7;

    // Jump variables
    public float jumpForce;
    public float gravityModifier;

    // Ground check boolean
    private bool isOnGround = false;

    // Sprite change gameObjects
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
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
        // Detect horizontal input
        moveX = Input.GetAxis("Horizontal");

        // Move player left or right depending on moveX and playerSpeed
        transform.Translate(Vector2.right * moveX * Time.deltaTime * playerSpeed);

        {
            // Call jump function if character jumps
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

    }

    // Move player with box if they are standing on it
    // Boxes have 90-degree platform effectors that let the player pass through their sides, but if they are touching the top of a box, they will stand on it
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Background"))
        {
            // Background is ground
            isOnGround = true;
        }
        // Square is box in Spawn Manager
        if (col.gameObject.name.Equals("Square"))
        {
            // square is ground
            isOnGround = true;

            // Player parents the square, and is dragged along with it
            transform.parent = col.transform;
        }
        if (col.gameObject.name.Equals("Coin"))
        {
            Destroy (col.gameObject);
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Background"))
        {
            // Background is ground
            isOnGround = false;
        }
        // Square = box in Spawn Manager
        if (col.gameObject.name.Equals("Square"))
        {
            // square = ground
            isOnGround = false;

            // Player unparents the square
            transform.parent = col.transform;
        }
    }
}
        
