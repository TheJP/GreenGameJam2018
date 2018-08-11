using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 0649
    public Vector2 StartPosition { get; set; }

    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float moveForce = 365f;

    [SerializeField]
    private float jumpForce = 1000f;

    [SerializeField]
    private Transform groundCheck;
#pragma warning restore 0649

    private bool jump = false;

    private Player player;
    private Rigidbody2D playerRigidBody2D;

    private int PlayerNumber => player?.PlayerNumber ?? 1;

    private void Awake()
    {
        if (Mathf.Abs(maxSpeed) < Mathf.Epsilon) { maxSpeed = 1.0f; }
        player = GetComponent<Player>();
        playerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() => this.transform.position = StartPosition;

    private void Update()
    {
        //So far all Buttons are only allowed if the Player stands on solid ground.
        if (IsPlayerOnGround())
        {
            if (Input.GetButtonDown($"Jump_{PlayerNumber}"))
            {
                this.playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
            }
        }

        if (Input.GetButtonDown($"Selector_{PlayerNumber}"))
        {
            Debug.Log($"Selector Button Down Occured from Player {PlayerNumber}");
        }

        if (Input.GetButtonDown($"Action_{PlayerNumber}"))
        {
            Debug.Log($"Action Button Down Occured from Player {PlayerNumber}");
        }

        if (Input.GetButtonDown($"Remove_{PlayerNumber}"))
        {
            Debug.Log($"Remove Button Down Occured from Player {PlayerNumber}");
        }
    }

    private void FixedUpdate()
    {
        if (IsHorizontalMovementAllowed())
        {
            var horizontal = Input.GetAxis($"Horizontal_{PlayerNumber}");
            playerRigidBody2D.AddForce(Vector2.right * horizontal * moveForce);

            if(Mathf.Abs(playerRigidBody2D.velocity.x) > maxSpeed)
            {
                playerRigidBody2D.velocity = new Vector2(Mathf.Sign(playerRigidBody2D.velocity.x) * maxSpeed, playerRigidBody2D.velocity.y);
            }
        }
    }

    private bool IsPlayerOnGround() => Physics2D.Linecast(transform.position, groundCheck.position, LayerMask.GetMask("Ground")).transform != null;

    private bool IsHorizontalMovementAllowed()
    {
        return true;
    }

    private bool IsVerticalMovementAllowed()
    {
        return true;
    }

    private bool IsJumpingAllowed()
    {
        return true;
    }
}