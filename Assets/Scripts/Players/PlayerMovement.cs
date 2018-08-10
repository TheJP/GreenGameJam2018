using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int PlayerNumber { get; set; }

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
            float translationHorizontal = Input.GetAxis("Horizontal") * maxVelocityHorizontal;
            currentPosition.x = currentPosition.x + translationHorizontal;
        }

        if (IsHorizontalMovementAllowed())
        {
            float translationVertical = Input.GetAxis("Vertical") * maxVelocityVertical;
            currentPosition.y = currentPosition.y + translationVertical;
        }

        transform.position = currentPosition;

        //Jump
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