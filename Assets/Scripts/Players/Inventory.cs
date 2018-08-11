﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySelection))]
public class Inventory : MonoBehaviour
{
    [SerializeField]
    public Canvas menuCanvas;

    [SerializeField]
    public Image spriteLeft;

    [SerializeField]
    public Image spriteMiddle;

    [SerializeField]
    public Image spriteRight;

#pragma warning disable 0649
    [SerializeField]
    private InventorySelection inventorySelection;
#pragma warning restore 0649

    private List<IInventoryItem> items;
    private int currentSelctedItem;
    private bool selectionChanged;

    public Player Player { get; set; }

    private void Awake()
    {
        items = new List<IInventoryItem>();
    }

    private void Start()
    {
        inventorySelection.ControllerNumber = Player.PlayerNumber;
    }

    private void Update()
    {
        if (selectionChanged)
        {
            selectionChanged = false;
            RedrawItems();
        }
    }

    public void EnableMenuView()
    {
        menuCanvas.enabled = true;
    }

    public void DisableMenuView()
    {
        menuCanvas.enabled = false;
    }

    public void EnableItemSelection()
    {
        inventorySelection.enabled = true;
    }

    public void DisableItemSelection()
    {
        inventorySelection.enabled = false;
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
        spriteLeft.sprite = (currentSelctedItem >= 1) ? items[currentSelctedItem - 1].Sprite : null;
        spriteMiddle.sprite = items[currentSelctedItem].Sprite;
        spriteRight.sprite = (currentSelctedItem < items.Count - 1) ? items[currentSelctedItem + 1].Sprite : null;
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
        Player?.ConfirmSelection(items[currentSelctedItem]);
    }

    public void CreateExampleItems()
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