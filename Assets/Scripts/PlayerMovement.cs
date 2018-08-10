using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPosition;
    private readonly float maxVelocityVertical = 1.0F;
    private readonly float maxVelocityHorizontal = 1.0F;

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