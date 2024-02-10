using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PlayerAniManager uses variables from THIS SCRIPT, so this short reminder is important for cross-script communication:

        // public variables / objects can be accessed in other scripts, and show in the Inspector.
        // Objects / variables marked with [SerializeField] will show in the inspector, even when private.
        // Objects / variables marked with [HideInInspector] will not show in the inspector, even when public.
        // private objects / variables can't be accessed in other scripts, and don't show in the inspector.

    // Rigidbody gameObject for jump physics. Implemented.
    public Rigidbody2D playerRb;
    public Animator animator;

    // Horizontal movement variables. Implemented.
    public int playerSpeed = 10;
    public float moveX;

    // Horizontal boundary variables. Implemented.
    public int leftBound = -9;
    public int rightBound = 7;

    // Jump variables. Implemented.
    public float jumpForce;
    public float gravityModifier;

    // Detects which specific box the player's BoxCollider2D is touching in any way, at any moment. This GameObject is called for two things:
    // When dropping through platform, ignore collision between this GameObject's collider and playerCollider for around 0.25 seconds or less. Then, reapply collision.
    // When sticking to platform while standing on the Floor, player becomes child of this GameObject if susMode (down key hold) is true.
    public GameObject currentOneWayPlatform;

    // Player's BoxCollider2D. Called when dropping through OneWayPlatform.
    [SerializeField] private BoxCollider2D playerCollider;

    // Ground check boolean. Called if the player is on ANY solid surface. Almost always paired with isJumping.
    public bool isOnGround = false;

    // Air check boolean. Called any time the player presses the jump key(s).
    public bool isJumping = false;

    // Static ground check boolean. Called any time the player is touching the floor, via standing on top of it.
    public bool touchGrass = false;

    // Down key hold boolean. Returns true if held. Locks other keys if true.
    public bool susMode = false;

    // Boolean to check if sprite should be flipped. Should be set to true if moveX < 0, and false if moveX > 0.
    public bool isFlipped = false;

    // The number of coins collected
    public int coinCount = 0;

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

        // Keep player in bounds
        // Also fix a bug where holding down on left bound lets player slide through left bound
        if (transform.position.x < leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, -2);

            if (transform.parent != null)
            {
                transform.parent = null;
            }
        }
        if (transform.position.x > rightBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, -2);

            if (transform.parent != null)
            {
                transform.parent = null;
            }
        }

        animator.SetBool("player_run", moveX > 0 || moveX < 0);
        animator.SetBool("player_jump", isJumping);
        animator.SetBool("player_jump_rise", playerRb.velocity.y > 0);
        animator.SetBool("player_sus", susMode);
    }

    void PlayerMove()
    {
        // susMode locks all other controls except for itself while down key is held
        if (susMode == false)
        {
            // Detect horizontal input
            moveX = Input.GetAxis("Horizontal");

            if (moveX < 0 && !isFlipped){
                isFlipped = true;
                Flip();
            } else if (moveX > 0 && isFlipped){
                isFlipped = false;
                Flip();
            }

            // Move player left or right depending on moveX and playerSpeed
            transform.Translate(Vector2.right * moveX * Time.deltaTime * playerSpeed);

            // Jump with spacebar
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                isOnGround = false;
            }
        }

        // Press and hold down to become SUS. Being SUS will change the way square colliders interact with the player on the ground and in the air, but take away all other controls.
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            // This bool locks the other controls while true.
            susMode = true;

            // If player is being sussy on the floor:
            if (touchGrass && susMode)
            {
                // If the player is being sussy on the floor AND their BoxCollider2D is touching a box, they will move with the box while down key is held.
                if (currentOneWayPlatform != null){
                    transform.parent = currentOneWayPlatform.transform;
                }
            }
        }
        // Only detach player from box ONCE if they let go of down, not ONCE PER FRAME
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            susMode = false;
            // this is overriding the transform.parent change when standing on boxes. This is not how I wanted this to interact.
            transform.parent = null;
        }

        // On the 1st frame down is pressed, and ONLY the first frame, do the following:
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {

            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

    }

    void Flip(){
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Move player with box if they are standing on it
    // Boxes have 90-degree platform effectors that let the player pass through their sides, so if they are touching the top of a box, they will stand on it
    void OnCollisionEnter2D(Collision2D col)
    {
        // Background is tagged as "Floor"
        if (col.gameObject.CompareTag("Floor"))
        {
            if (isJumping)
            {
                // Reset jump if player is jumping
                isJumping = false;
            }

            // Floor is ground
            isOnGround = true;

            // Floor is "grass"
            touchGrass = true;
        }
        // Square is tagged as "OneWayPlatform"
        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            // currentOneWayPlatform detects the box the player is currently on, and marks it.
            currentOneWayPlatform = col.gameObject;
            if (isJumping)
            {
                // Reset jump if player is jumping
                isJumping = false;
            }

            // square is ground
            isOnGround = true;

            // If player is touching box, but NOT touching floor. So:
            // If player is standing on box:
            if (touchGrass == false)
            {
                // Player parents the box, and is dragged along with it. In-game, this lets the player "ride" the moving boxes by standing on them.
                transform.parent = col.transform;
            }
        }

        /* Removed due to being a trigger instead of a physics object
        if (col.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Collided with coin");
            coinCount++;
        }
        */

    }
    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collided with coin");
        coinCount++;
        Destroy(col.gameObject);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // If player jumped off the floor
        if (col.gameObject.CompareTag("Floor") && isJumping)
        {
            // Player is no longer touching the floor
            touchGrass = false;
        }
        // Square = box in Spawn Manager
        if (col.gameObject.CompareTag("OneWayPlatform"))

            // When the player stops touching currentOneWayPlatform, the box they were on stops being marked as a GameObject in this script.
            currentOneWayPlatform = null;
        {
            //If player jumped off a box:
            if (isJumping)
            {
                // square is ground
                isOnGround = false;
                //Player unparents the collider they were standing on.
                transform.parent = null;
            }


            // If player didn't JUMP off a box, but they're not touching the floor either. This happens if you walk off the edge of a box.
            if (touchGrass == false)
            {
                // Player still unparents the collider they were standing on as if they had jumped off it.
                // Sliding with boxes on the ground will call for a different transform.parent; currentOneWayPlatform.
                transform.parent = null;
                isOnGround = false;
            }

        }
    }
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}