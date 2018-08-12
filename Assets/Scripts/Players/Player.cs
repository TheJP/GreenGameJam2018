using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] private int playerNumber;

    public int PlayerNumber
    {
        get { return this.playerNumber; }
        set { this.playerNumber = value; }
    }

    enum DeathAnimationState
    {
        Alive,
        Dying,
        Dead
    }

#pragma warning disable 0649
    [SerializeField] private Sprite sprite;

    [SerializeField] private Oxygen oxygenUsagePerSecond = new Oxygen(5);

    [SerializeField] private Oxygen oxygenStorageCapacity = new Oxygen(100);

    [SerializeField] private GameObject inventoryPrefab;

    [SerializeField] private Slider oxygenBar;

    [SerializeField] private SpriteRenderer leftHand;

    [SerializeField] private SpriteRenderer rightHand;

    [SerializeField] private FadeoutText fadeoutTextPrefab;

    [SerializeField] private DeathAnimationState _deathAnimationDeathAnimationState = DeathAnimationState.Alive;

#pragma warning restore 0649

    private Inventory inventory;

    public PrefabTile[] tiles;
    private TileController tileController;

    private ResourceManager resourceManager;

    public GameObject[] weapons;
    public GameObject weaponCache;
    public ParticleSystem DeathParticleSystem;

    [SerializeField] private Color color;

    public Color Color
    {
        get { return this.color; }
        set { this.color = value; }
    }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public PlayerAttributes Attributes { get; private set; }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.animator = GetComponentInChildren<Animator>();

        Attributes = new PlayerAttributes(oxygenStorageCapacity);
        Attributes.OxygenLevelChanged += OxygenLevelChanged;

        GameObject inventoryObject = Instantiate(inventoryPrefab, transform);
        this.inventory = inventoryObject.GetComponent<Inventory>();

        this.tileController = FindObjectOfType<TileController>();

        this.resourceManager = FindObjectOfType<ResourceManager>();
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

            bool lookRight = Input.GetAxis($"Horizontal_{PlayerNumber}") > 0;
            this.spriteRenderer.flipX = (lookRight) ? false : true;

            DisplayCurrentItem();
        }
        else
        {
            this.animator.enabled = false;
        }

        if (!Attributes.IsAlive && _deathAnimationDeathAnimationState == DeathAnimationState.Alive)
        {
            _deathAnimationDeathAnimationState = DeathAnimationState.Dying;
            for (int i = 0; i < 3; i++)
            {
                var particles = Instantiate(DeathParticleSystem);
                particles.transform.SetParent(transform, false);
                particles.Play();
            }
        }
    }

    private void DisplayCurrentItem()
    {
        if (Attributes.CurrentEquippedItem != null)
        {
            bool lookRight = Input.GetAxis($"Horizontal_{PlayerNumber}") > 0;

            if (Attributes.CurrentEquippedItem is InventoryTile)
            {
                rightHand.sprite = lookRight ? Attributes.CurrentEquippedItem.Sprite : null;
                leftHand.sprite = lookRight ? null : Attributes.CurrentEquippedItem.Sprite;
            }
            else if (Attributes.CurrentEquippedItem is InventoryWeapon)
            {
                GameObject weapon = ((InventoryWeapon) Attributes.CurrentEquippedItem).Weapon;
                if (lookRight)
                {
                    weapon.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    weapon.gameObject.transform.SetParent(rightHand.gameObject.transform, false);
                }
                else
                {
                    weapon.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
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
        if (!Attributes.IsAlive)
        {
            return Oxygen.Zero;
        }
        else
        {
            return Attributes.MaxOxygen - Attributes.CurrentOxygen;
        }
    }

    /// <summary>Receive oxygen to refill the oxygen tank.</summary>
    /// <param name="oxygen">Amount of oxygen that this player receives.</param>
    /// <returns>True if the oxygen could be accepted, false otherwise.</returns>
    public bool Receive(Oxygen oxygen)
    {
        if (!Attributes.IsAlive)
        {
            return false;
        }
        else if (oxygen > Attributes.MaxOxygen - Attributes.CurrentOxygen)
        {
            return false;
        }
        else
        {
            // TODO: Add oxygen received animation
            Attributes.IncreaseOxygen(oxygen);
            return true;
        }
    }

    /// <summary>
    /// Opens the Menu with the Inventory with the available Tiles and Weapons.
    /// </summary>
    internal void OpenInventory()
    {
        if (Attributes.IsAlive)
        {
            this.playerMovement.enabled = false;
            var itemList = new List<IInventoryItem>();

            foreach (GameObject weapon in weapons)
            {
                Sprite sprite = weapon.GetComponent<SpriteRenderer>().sprite;
                IInventoryItem item = new InventoryWeapon(Instantiate(weapon, weaponCache.transform), sprite);
                itemList.Add(item);
            }

            foreach (PrefabTile tile in tiles)
            {
                IInventoryItem item = new InventoryTile(tile);
                itemList.Add(item);
            }

            this.inventory.EnableMenuView(itemList);
            this.inventory.EnableItemSelection();
        }
    }

    internal void ConfirmSelection(IInventoryItem inventoryItem)
    {
        this.inventory.DisableItemSelection();
        this.inventory.DisableMenuView();

        Attributes.CurrentEquippedItem = this.inventory.GetCurrentSelectedItem();
        ClearHands();
        DisplayCurrentItem();

        this.playerMovement.enabled = true;
    }

    /// <summary>
    /// Executes an Action depending on the CurrentEquipped Item. (Build or use Weapon)
    /// </summary>
    internal void ProcessAction()
    {
        if (Attributes.IsAlive)
        {
            IInventoryItem currentItem = Attributes.CurrentEquippedItem;
            if (currentItem is InventoryTile)
            {
                InventoryTile inventoryTile = (InventoryTile) currentItem;
                PrefabTile tileToBuild = inventoryTile.Tile;

                if (this.resourceManager.ConstructionMaterialAvailable >= tileToBuild.BuildingCosts)
                {
                    if (!(this.resourceManager.TryConsume(tileToBuild.BuildingCosts) &&
                          this.tileController.TryAddTile(tileToBuild, transform.position)))
                    {
                        var fadeout = Instantiate(fadeoutTextPrefab, transform);
                        fadeout.TextMesh.text = "#@!&$%";
                        fadeout.TextMesh.color = new Color(1.0f, 0.0f, 0.0f, 1);
                    }
                    else
                    {
                        var fadeout = Instantiate(fadeoutTextPrefab, transform);
                        fadeout.TextMesh.text = $"-{(float)tileToBuild.BuildingCosts}";
                        fadeout.TextMesh.color = new Color(0.6392157F, 0.5019608F, 0.3892157F, 1.0F);
                    }
                }
                else
                {
                    var fadeout = Instantiate(fadeoutTextPrefab, transform);
                    fadeout.TextMesh.text = "Not enought Resources!";
                    fadeout.TextMesh.color = new Color(1.0f, 0.0f, 0.0f, 1);
                }
            }
            else if (currentItem is InventoryWeapon)
            {
                InventoryWeapon inventoryWeapon = (InventoryWeapon) currentItem;
                inventoryWeapon.Weapon.GetComponent<IPlayerWeapon>().Fire();
            }
        }
    }

    internal void SellTower()
    {
        if (!Attributes.IsAlive) { return; }

        if (!TowerSellable()) { return; }

        PrefabTile tile;
        if (tileController.TryRemoveTile(transform.position, out tile))
        {
            resourceManager.Store(tile.BuildingCosts / 2);
            var fadeout = Instantiate(fadeoutTextPrefab, transform);
            fadeout.TextMesh.text = $"+{(float)tile.BuildingCosts / 2}";
            fadeout.TextMesh.color = new Color(0.6392157F, 0.5019608F, 0.3892157F, 1.0F);
        }
    }

    private bool TowerSellable()
    {
        //TODO: Check if Tower is allowed to sell. Maybe the initial towers can be tagged?
        return true;
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