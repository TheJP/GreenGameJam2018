using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } set { this.playerNumber = value; } }

    public Vector2 StartPosition { get; set; }

    [SerializeField]
    private float maxVelocityVertical;

    [SerializeField]
    private float maxVelocityHorizontal;

    [SerializeField]
#pragma warning disable 0649
    private Rigidbody2D rigidbody2d;
#pragma warning restore 0649

    private Vector2 currentPosition;

    public const int JumpHeight = 50;

    void Update()
    {
        if (IsVerticalMovementAllowed())
        {
            float translationHorizontal = Input.GetAxis($"Horizontal_{PlayerNumber}") * maxVelocityHorizontal * Time.deltaTime;
            currentPosition.x = currentPosition.x + translationHorizontal;
            transform.position = new Vector2(currentPosition.x, transform.position.y);
        }

        //For climbing Ladders
        //if (IsHorizontalMovementAllowed())
        //{
        //    float translationVertical = Input.GetAxis($"Vertical_{PlayerNumber}") * maxVelocityVertical * Time.deltaTime;
        //    currentPosition.y = currentPosition.y + translationVertical;
        //}

        //So far all Buttons are only allowed if the Player stands on solid ground.
        if (IsPlayerOnGround())
        {
            if (Input.GetButtonDown($"Jump_{PlayerNumber}"))
            {
                this.rigidbody2d.AddForce(new Vector2(0, JumpHeight));
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
    }

    private bool IsPlayerOnGround()
    {
        //TODO: Check if the Player stands on solid Ground
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void Initilaize()
    {
        this.currentPosition = StartPosition;
        this.transform.position = StartPosition;

        if (this.maxVelocityVertical == 0)
        {
            maxVelocityVertical = 1.0F;
        }

        if (maxVelocityHorizontal == 0)
        {
            maxVelocityHorizontal = 1.0F;
        }
    }

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

    //private IEnumerator Jump()
    //{
    //    while (transform.position.y < JumpHeight)
    //    {

    //    }
    //    yield return null;
    //}
}