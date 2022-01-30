using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private int facingDirection = 1;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallHopForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float airMovementForce;
    [SerializeField] private float airDragMultiplier = 0.95f;

    public Transform GroundCheck;
    public Transform WallCheck;
    public LayerMask WhatIsGround;


    private bool isWalking = false;
    private bool isGrounded = false;
    private bool isTouchingWall = false;
    private bool canJump = false;
    private bool isWallSliding;

    private float moveDirection;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }


    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            Debug.Log("moving 1");
            rigidbody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidbody2D.velocity.y);
        }
        else if (!isGrounded && !isWallSliding && moveDirection != 0)
        {
            Debug.Log("moving 2");

            Vector2 forceToAdd = new Vector2(airMovementForce * moveDirection, 0);
            rigidbody2D.AddForce(forceToAdd);

            if (Mathf.Abs(rigidbody2D.velocity.x) > moveSpeed)
            {
                rigidbody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidbody2D.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && moveDirection == 0)
        {
            Debug.Log("moving 3");

            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * airDragMultiplier, rigidbody2D.velocity.y);
        }

        if (isWallSliding)
        {
            if (rigidbody2D.velocity.y < -wallSlideSpeed)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }
        else if (isWallSliding && moveDirection == 0 && canJump)
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rigidbody2D.AddForce(forceToAdd, ForceMode2D.Impulse);

        }
        else if ((isWallSliding || isTouchingWall) && moveDirection != 0 && canJump)
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * moveDirection, wallJumpForce * wallJumpDirection.y);
            rigidbody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
            if (facingDirection == 1)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rigidbody2D.velocity.y <= 0) || isWallSliding)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rigidbody2D.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("yVelocity", rigidbody2D.velocity.y);
        animator.SetBool("IsWallSliding", isWallSliding);
    }

    private void CheckMovementDirection()
    {
        if (moveDirection > 0)
        {
            isWalking = true;
            facingDirection = 1;
            if (!isWallSliding)
            {
                transform.eulerAngles = Vector3.zero;
            }

        }
        else if (moveDirection < 0)
        {
            isWalking = true;
            facingDirection = -1;
            if (!isWallSliding)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
        else
        {
            isWalking = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, WhatIsGround);
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, WhatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }

}
