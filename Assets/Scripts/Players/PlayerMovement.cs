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

    private Vector2 currentPosition;

    void Update()
    {
        if (IsVerticalMovementAllowed())
        {
            //Flip Sprite Direction
            float translationHorizontal = Input.GetAxis($"Horizontal_{PlayerNumber}") * maxVelocityHorizontal * Time.deltaTime;
            currentPosition.x = currentPosition.x + translationHorizontal;
        }

        if (IsHorizontalMovementAllowed())
        {
            float translationVertical = Input.GetAxis($"Vertical_{PlayerNumber}") * maxVelocityVertical * Time.deltaTime;
            currentPosition.y = currentPosition.y + translationVertical;
        }

        transform.position = currentPosition;

        if (Input.GetButtonDown($"Jump_{PlayerNumber}"))
        {
            Debug.Log($"Jump Button Down Occured from Player {PlayerNumber}");
        }

        if (Input.GetButtonDown($"Action_{PlayerNumber}"))
        {
            Debug.Log($"Action Button Down Occured from Player {PlayerNumber}");
        }

        if (Input.GetButtonDown($"Selector_{PlayerNumber}"))
        {
            Debug.Log($"Selector Button Down Occured from Player {PlayerNumber}");
        }

        if (Input.GetButtonDown($"Remove_{PlayerNumber}"))
        {
            Debug.Log($"Remove Button Down Occured from Player {PlayerNumber}");
        }
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
}