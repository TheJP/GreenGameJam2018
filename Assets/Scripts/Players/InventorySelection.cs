using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelection : MonoBehaviour
{

    private Inventory inventory;
    public int ControllerNumber { get; set; }

    private bool selectionMoved;

    void Start()
    {
        this.inventory = GetComponent<Inventory>();
        if (ControllerNumber == 0)
        {
            ControllerNumber = 1;
        }
    }

    void Update()
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
        }
        else if (horizontal > 0 && !selectionMoved)
        {
            selectionMoved = true;
            inventory.MoveSelectionRight();
        }
        else
        {
            selectionMoved = false;
        }
    }
}
