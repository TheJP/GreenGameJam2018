using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int playerNumber;

    [SerializeField]
    private Vector2 startPosition;

    [SerializeField]
    private float maxVelocityVertical;

    [SerializeField]
    private float maxVelocityHorizontal;

    private Vector2 currentPosition;

    void Start()
    {
        this.currentPosition = startPosition;

        if (this.maxVelocityVertical == 0)
        {
            maxVelocityVertical = 1.0F;
        }

        if (maxVelocityHorizontal == 0)
        {
            maxVelocityHorizontal = 1.0F;
        }
    }

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