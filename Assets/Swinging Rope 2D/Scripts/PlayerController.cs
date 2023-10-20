using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    // Get references to rigidbody and player collider
    Rigidbody2D rb2d;
    BoxCollider2D boxCollider2D;

    // Layermask used to detect only the assigned layers in the inspector
    public LayerMask platformLayerMask;

    // Variables used to deal with user inputs
    float horizontalInput;
    bool facingRight;
    bool walking;
    public float jumpHeight = 20;
    public float movementSpeed = 5;

    // Declare the bool that checks if the player is already attached to a rope
    [HideInInspector] public bool alreadyConnected = false;

    // Declare the animator component
    [HideInInspector] public Animator animator;

    #endregion

    #region Unity Methods

    void Start()
    {
        // Assign the components
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is grounded and player pressed the spacebar before jumping
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Jump(jumpHeight);
        }

        // Read the user input
        horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        // Changes the local scale based on the facing direction
        AdjustDirection();
    }

    void FixedUpdate()
    {
        if (walking)
        {
            // Move the player in this method
            Walk();
        }
    }
    #endregion

    #region Custom Methods

    /// <summary>
    ///  Checks if player is touching the ground
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        // perform a boxcast under the character to detect the ground assigned in the layer mask
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center,
            new Vector2(boxCollider2D.bounds.size.x, 0.01f), 0f, Vector2.down,
            boxCollider2D.bounds.extents.y + 0.01f, platformLayerMask);

        return raycastHit2D.collider != null;
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    /// <param name="jumpHeight"></param>
    public void Jump(float jumpHeight)
    {
        // Change the velocity of the rigidbody to move the character vertically
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
    }

    /// <summary>
    /// Moves the player based on user input
    /// </summary>
    void Walk()
    {
        // Change the velocity of the rigidbody to move the character horizontally
        rb2d.velocity = new Vector2(horizontalInput * movementSpeed, rb2d.velocity.y);

        // Check the facing direction based on user input
        if (horizontalInput > 0)
        {
            facingRight = true;
        }
        else if (horizontalInput < 0)
        {
            facingRight = false;
        }
    }

    /// <summary>
    /// Changes the local scale based on user input
    /// </summary>
    void AdjustDirection()
    {
        if (facingRight)
        {
            // Change the X value of the transform scale to its positive value when facing right
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        else
        {
            // Change the X value of the transform scale to its negative value when facing left
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    /// <summary>
    /// Play the right animations and effects based on player state on rope
    /// </summary>
    /// <param name="playerMovementOnRope"></param>
    public void HandleRopeAnimationsAndSoundEffects(PlayerMovementOnRope playerMovementOnRope)
    {
        /* I would recommend directly playing the animations by using animator.Play() . Then you can use the if statements that contain
        the DismountRope method in the update function of the last rope script to play the default animations after dismounting.
        For more information, please read question 21 the documentation provided */

        switch (playerMovementOnRope)
        {
            case PlayerMovementOnRope.notMoving:
                // Play the player idle animation and sound effect on rope here
                Debug.Log("Not Moving");
                animator.Play("");

                break;
            case PlayerMovementOnRope.climbing:
                // Play the player climbing animation and sound effect on rope here
                Debug.Log("Climbing");
                animator.Play("");

                break;
            case PlayerMovementOnRope.falling:
                // Play the player falling animation and sound effect on rope here
                Debug.Log("Falling");
                animator.Play("");

                break;
        }

    }
    #endregion
}
public enum PlayerMovementOnRope
{
    notMoving,
    climbing,
    falling,
}
