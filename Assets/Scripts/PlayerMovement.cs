using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPosition;
    private readonly float maxVelocityVertical = 1.0F;
    private readonly float maxVelocityHorizontal = 1.0F;

    [SerializeField]
    private Vector2 currentPosition;

    // Use this for initialization
    void Start()
    {
        this.currentPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsVerticalMovementAllowed())
        {
            //Flip Sprite Direction
            //StartAnimation
            float translationVertical = Input.GetAxis("Vertical") * maxVelocityVertical;
            currentPosition.x = currentPosition.x + translationVertical;
        }

        if (IsHorizontalMovementAllowed())
        {
            //StartAnimation
            float translationHorizontal = Input.GetAxis("Horizontal") * maxVelocityHorizontal;
            currentPosition.y = currentPosition.y + translationHorizontal;
        }

        //StopAnimation

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