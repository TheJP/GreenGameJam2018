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
        if (Mathf.Abs(this.maxSpeed) < Mathf.Epsilon) { this.maxSpeed = 1.0f; }
        this.player = GetComponent<Player>();
        this.playerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() => this.transform.position = this.StartPosition;

    private void Update()
    {
        //So far all Buttons are only allowed if the Player stands on solid ground.
        if (IsPlayerOnGround() && IsJumpingAllowed() && Input.GetButtonDown($"Jump_{PlayerNumber}"))
        {
            jump = true;
        }

        if (Input.GetButtonDown($"Selector_{PlayerNumber}"))
        {
            Debug.Log($"Selector Button Down Occured from Player {PlayerNumber}");
            player.OpenInventory();
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

        if (jump)
        {
            this.playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
    }

    private bool IsPlayerOnGround() => Physics2D.Linecast(transform.position, groundCheck.position, LayerMask.GetMask("Ground")).transform != null;

    private bool IsHorizontalMovementAllowed()
    {
        return player.Attributes.IsAlive;
    }

    private bool IsJumpingAllowed()
    {
        return player.Attributes.IsAlive;
    }
}