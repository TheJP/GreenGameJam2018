using Resources;
using System.Collections;
using UnityEngine;

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
#pragma warning restore 0649

    [SerializeField]
    private Color color;
    public Color Color { get { return this.color; } set { this.color = value; } }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public PlayerAttributes Attributes { get; private set; }

    private void Awake()
    {
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.animator = GetComponentInChildren<Animator>();

        Attributes = new PlayerAttributes(this, oxygenStorageCapacity);
    }

    private void Start()
    {
        this.spriteRenderer.sprite = this.sprite;
        this.spriteRenderer.color = this.color;

        Attributes.IsAlive = true;
    }

    private void Update()
    {
        if (Input.GetAxis($"Horizontal_{PlayerNumber}") != 0 || Input.GetAxis($"Vertical_{PlayerNumber}") != 0)
        {
            this.spriteRenderer.flipX = (Input.GetAxis($"Horizontal_{PlayerNumber}") > 0) ? false : true;
            this.animator.enabled = true;
        }
        else
        {
            this.animator.enabled = false;
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
}