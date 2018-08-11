using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;
    public int PlayerNumber { get { return this.playerNumber; } set { this.playerNumber = value; } }

#pragma warning disable 0649
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private Oxygen oxygenUsagePerSecond = new Oxygen(5);

    [SerializeField]
    private Oxygen oxygenStorageCapacity = new Oxygen(100);

    [SerializeField]
    private GameObject inventoryPrefab;

    [SerializeField]
    private Slider oxygenBar;

    [SerializeField]
    private SpriteRenderer leftHand;

    [SerializeField]
    private SpriteRenderer rightHand;
#pragma warning restore 0649

    private Inventory inventory;

    public PrefabTile[] tiles;
    private TileController tileController;

    public GameObject[] weapons;
    public GameObject weaponCache;

    [SerializeField]
    private Color color;
    public Color Color { get { return this.color; } set { this.color = value; } }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public PlayerAttributes Attributes { get; private set; }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.animator = GetComponentInChildren<Animator>();

        Attributes = new PlayerAttributes(this, oxygenStorageCapacity);
        Attributes.OxygenLevelChanged += OxygenLevelChanged;

        GameObject inventoryObject = Instantiate(inventoryPrefab, transform);
        this.inventory = inventoryObject.GetComponent<Inventory>();

        this.tileController = FindObjectOfType<TileController>();
    }

    private void Start()
    {
        this.spriteRenderer.sprite = this.sprite;
        this.spriteRenderer.color = this.color;
        this.playerMovement = GetComponent<PlayerMovement>();

        this.inventory.Player = this;
        this.inventory.DisableItemSelection();
        this.inventory.DisableMenuView();

        Attributes.IsAlive = true;
    }

    private void Update()
    {
        if (Input.GetAxis($"Horizontal_{PlayerNumber}") != 0)
        {
            this.animator.enabled = true;
            DisplayCurrentItem();
        }
        else
        {
            this.animator.enabled = false;
        }
    }

    private void DisplayCurrentItem()
    {
        if (Attributes.CurrentEquippedItem != null)
        {
            if (Attributes.CurrentEquippedItem is InventoryTile)
            {
                bool lookRight = Input.GetAxis($"Horizontal_{PlayerNumber}") > 0;
                this.spriteRenderer.flipX = (lookRight) ? false : true;

                rightHand.sprite = lookRight ? Attributes.CurrentEquippedItem.Sprite : null;
                leftHand.sprite = lookRight ? null : Attributes.CurrentEquippedItem.Sprite;
            }
            else if (Attributes.CurrentEquippedItem is InventoryWeapon)
            {
                GameObject weapon = ((InventoryWeapon)Attributes.CurrentEquippedItem).Weapon;

                bool lookRight = Input.GetAxis($"Horizontal_{PlayerNumber}") > 0;
                if (lookRight)
                {
                    weapon.GetComponent<SpriteRenderer>().flipX = false;
                    weapon.gameObject.transform.SetParent(rightHand.gameObject.transform, false);
                }
                else
                {
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                    weapon.gameObject.transform.SetParent(leftHand.gameObject.transform, false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Attributes.DecreseOxygen(oxygenUsagePerSecond * Time.fixedDeltaTime);
    }

    /// <summary>Calculates how much oxygen this player maximally receive.</summary>
    public Oxygen MaxReceiveOxygen()
    {
        if (!Attributes.IsAlive) { return Oxygen.Zero; }
        else { return Attributes.MaxOxygen - Attributes.CurrentOxygen; }
    }

    /// <summary>Receive oxygen to refill the oxygen tank.</summary>
    /// <param name="oxygen">Amount of oxygen that this player receives.</param>
    /// <returns>True if the oxygen could be accepted, false otherwise.</returns>
    public bool Receive(Oxygen oxygen)
    {
        if (!Attributes.IsAlive) { return false; }
        else if (oxygen > Attributes.MaxOxygen - Attributes.CurrentOxygen) { return false; }
        else
        {
            // TODO: Add oxygen received animation
            Attributes.IncreaseOxygen(oxygen);
            return true;
        }
    }

    internal void OpenInventory()
    {
        this.playerMovement.enabled = false;

        var itemList = new List<IInventoryItem>();
        foreach (PrefabTile tile in tiles)
        {
            IInventoryItem item = new InventoryTile(tile);
            itemList.Add(item);
        }

        foreach (GameObject weapon in weapons)
        {
            Sprite sprite = weapon.GetComponent<SpriteRenderer>().sprite;
            IInventoryItem item = new InventoryWeapon(Instantiate(weapon, weaponCache.transform), sprite);
            itemList.Add(item);
        }


        this.inventory.EnableMenuView(itemList);
        this.inventory.EnableItemSelection();
    }

    internal void ConfirmSelection(IInventoryItem inventoryItem)
    {
        Debug.Log($"Current Item with Sprite {this.inventory.GetCurrentSelectedItem().Sprite.name} is confirmed");
        this.inventory.DisableItemSelection();
        this.inventory.DisableMenuView();

        Attributes.CurrentEquippedItem = this.inventory.GetCurrentSelectedItem();
        ClearHands();

        this.playerMovement.enabled = true;
    }

    internal void ProcessAction()
    {
        IInventoryItem currentItem = Attributes.CurrentEquippedItem;
        if (currentItem is InventoryTile)
        {
            InventoryTile inventoryTile = (InventoryTile)currentItem;
            tileController.TryAddTile(inventoryTile.Tile, transform.position);
        }
        else if (currentItem is InventoryWeapon)
        {
            InventoryWeapon inventoryWeapon = (InventoryWeapon)currentItem;
            inventoryWeapon.Weapon.GetComponent<IPlayerWeapon>().Fire();
        }
    }

    internal void SellTower()
    {
        // TODO: Sell Tower in front of you
    }

    private void OxygenLevelChanged(Oxygen oxygen)
    {
        oxygenBar.gameObject.SetActive(Attributes.MaxOxygen - oxygen >= oxygenUsagePerSecond);
        oxygenBar.normalizedValue = oxygen / Attributes.MaxOxygen;
    }

    private void ClearHands()
    {
        leftHand.sprite = null;
        rightHand.sprite = null;

        foreach (Transform child in leftHand.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in rightHand.transform)
        {
            Destroy(child.gameObject);
        }
    }
}