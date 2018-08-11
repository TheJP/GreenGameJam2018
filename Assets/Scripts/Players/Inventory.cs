using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    [SerializeField]
    private int controllerNumber;
    public int ControllerNumber { get { return controllerNumber; } set { this.controllerNumber = value; } }

    [SerializeField]
    public Canvas menuCanvas;

    [SerializeField]
    public Image spriteLeft;

    [SerializeField]
    public Image spriteMiddle;

    [SerializeField]
    public Image spriteRight;

    private List<IInventoryItem> items;
    private int currentSelctedItem;
    private bool selectionChanged;
    private InventorySelection inventorySelection;

    public Player player { get; set; }

    // Use this for initialization
    void Start()
    {
        items = new List<IInventoryItem>();
        CreateExampleItems();

        inventorySelection = GetComponent<InventorySelection>();
        inventorySelection.ControllerNumber = ControllerNumber;
    }

    void Update()
    {
        if (selectionChanged)
        {
            selectionChanged = false;
            RedrawItems();
        }
    }

    public void EnableMenu()
    {
        menuCanvas.enabled = true;
    }

    public void DisableMenu()
    {
        menuCanvas.enabled = false;
    }

    public void AddItems(IList<IInventoryItem> items)
    {
        this.items.AddRange(items);
        selectionChanged = true;
    }

    public IInventoryItem GetCurrentSelectedItem()
    {
        return items[currentSelctedItem];
    }

    private void RedrawItems()
    {
        if (currentSelctedItem >= 1)
        {
            spriteLeft.sprite = items[currentSelctedItem - 1].Sprite;
        }
        else
        {
            spriteLeft.sprite = null;
        }

        spriteMiddle.sprite = items[currentSelctedItem].Sprite;

        if (currentSelctedItem < items.Count - 1)
        {
            spriteRight.sprite = items[currentSelctedItem + 1].Sprite;
        }
        else
        {
            spriteRight.sprite = null;
        }
    }

    internal void MoveSelectionRight()
    {
        if (currentSelctedItem < items.Count - 1)
        {
            selectionChanged = true;
            currentSelctedItem++;
        }
        else
        {
            currentSelctedItem = items.Count - 1;
        }
    }

    internal void MoveSelcetionLeft()
    {
        if (currentSelctedItem > 0)
        {
            selectionChanged = true;
            currentSelctedItem--;
        }
        else
        {
            currentSelctedItem = 0;
        }
    }

    internal void Confirm()
    {
        player?.ConfirmSelection(items[currentSelctedItem]);
    }

    private void CreateExampleItems()
    {
        IInventoryItem item1 = new InventoryWeapon(UnityEngine.Resources.Load<Sprite>("weapon_club"));
        IInventoryItem item2 = new InventoryWeapon(UnityEngine.Resources.Load<Sprite>("weapon_gun"));
        IInventoryItem item3 = new InventoryWeapon(UnityEngine.Resources.Load<Sprite>("weapon_saw"));
        List<IInventoryItem> items = new List<IInventoryItem>();
        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        AddItems(items);
    }
}