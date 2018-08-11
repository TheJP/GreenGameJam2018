using System.Collections;
using UnityEngine;

public class InventorySelection : MonoBehaviour
{

    private Inventory inventory;
    public int ControllerNumber { get; set; }

    private bool selectionMoved;
    private const float MenuMoveCoolDownTime = 0.26F;

    private void Start()
    {
        this.inventory = GetComponent<Inventory>();
        if (ControllerNumber == 0)
        {
            ControllerNumber = 1;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown($"Jump_{ControllerNumber}"))
        {
            inventory.Confirm();
        }

        //TODO: Add abort button and select last selected Item.
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis($"Horizontal_{ControllerNumber}");
        if (horizontal < 0 && !selectionMoved)
        {
            selectionMoved = true;
            inventory.MoveSelcetionLeft();
            StartCoroutine(CoolDownNextMovement());
        }
        else if (horizontal > 0 && !selectionMoved)
        {
            selectionMoved = true;
            inventory.MoveSelectionRight();
            StartCoroutine(CoolDownNextMovement());
        }
    }

    private IEnumerator CoolDownNextMovement()
    {
        yield return new WaitForSecondsRealtime(MenuMoveCoolDownTime);
        selectionMoved = false;
    }
}
