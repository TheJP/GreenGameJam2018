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
#pragma warning restore 0649

    [SerializeField]
    private Color color;
    public Color Color { get { return this.color; } set { this.color = value; } }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int currentOxygen;
    private readonly float OXYGEN_REFILL_DELAY = 0.5F;
    private readonly float OXYGEN_CONSUME_DELAY = 1.0F;
    private Coroutine oxygenRefill;
    private Coroutine oxygenConsumer;

    public int MaxOxygen { get; set; }
    public bool IsAlive { get; private set; }

    //Phobie
    //Equipped Weapon
    //Inventory

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        if (MaxOxygen == 0)
        {
            MaxOxygen = 100;
        }
        this.currentOxygen = MaxOxygen;

        this.spriteRenderer.sprite = this.sprite;
        this.spriteRenderer.color = this.color;

        IsAlive = true;
        StartConsumeOxygen();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            this.spriteRenderer.flipX = (Input.GetAxis("Horizontal") > 0) ? false : true;
            this.animator.enabled = true;
        }
        else
        {
            this.animator.enabled = false;
        }
    }

    /*
     * Increase the Oxygen of this Player about the given amount. 
     * If the Oxygen is already on maxOxygen, this call is ignored.
     */
    public void IncreaseOxygen(int amount)
    {
        if (this.currentOxygen < MaxOxygen)
        {
            currentOxygen++;
        }
    }

    /*
     * Decrease the Oxygen of this Player about the given amount.
     * If the Oxygen is 0 (or lower) IsAlive is set to false;
     */
    public void DecreseOxygen(int amount)
    {
        if (this.currentOxygen > 0)
        {
            this.currentOxygen--;
        }
        else
        {
            IsAlive = false;
            StopRefillOxgen();
            StopConsumeOxygen();
        }
    }

    /*
     * Start a Coroutine which consumes continuously oxgen.
     * At the moment, only one of these coroutines at the time can be started.
     */
    public void StartConsumeOxygen()
    {
        StopConsumeOxygen();
        this.oxygenConsumer = StartCoroutine(ConsumeOxygen());
    }

    /*
     * Stops the Coroutine which consumes continuously oxgen if running.
     */
    public void StopConsumeOxygen()
    {
        if (this.oxygenConsumer != null)
        {
            StopCoroutine(oxygenConsumer);
        }
    }

    /*
     * Start a Coroutine which refills continuously oxgen.
     * At the moment, only one of these coroutines at the time can be started.
     */
    public void StartRefillOxygen()
    {
        this.oxygenRefill = StartCoroutine(RefillOxygen());
    }

    /*
     * Stops the Coroutine which refills continuously oxgen if running.
     */
    public void StopRefillOxgen()
    {
        if (this.oxygenRefill != null)
        {
            StopCoroutine(oxygenRefill);
        }
    }

    private IEnumerator RefillOxygen()
    {
        while (true)
        {
            IncreaseOxygen(1);
            yield return new WaitForSeconds(OXYGEN_REFILL_DELAY);
        }
    }

    private IEnumerator ConsumeOxygen()
    {
        while (true)
        {
            DecreseOxygen(1);
            yield return new WaitForSeconds(OXYGEN_CONSUME_DELAY);
        }
    }
}