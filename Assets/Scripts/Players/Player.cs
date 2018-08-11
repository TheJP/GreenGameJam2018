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

    public int MaxOxygen { get { return attributes.MaxOxygen; } set { attributes.MaxOxygen = value; } }
    private PlayerAttributes attributes;

    private const float OxygenRefillDelay = 0.5F;
    private const float OxygenConsumeDelay = 1.0F;
    private Coroutine oxygenRefill;
    private Coroutine oxygenConsumer;

    private void Awake()
    {
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.animator = GetComponentInChildren<Animator>();

        attributes = new PlayerAttributes(this);
    }

    public void Initialize()
    {
        this.spriteRenderer.sprite = this.sprite;
        this.spriteRenderer.color = this.color;

        attributes.IsAlive = true;
        StartConsumeOxygen();
    }

    void Update()
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

    /// <summary>
    /// Start a Coroutine which consumes continuously oxgen.
    /// At the moment, only one of these coroutines at the time can be started.
    /// </summary>
    public void StartConsumeOxygen()
    {
        StopConsumeOxygen();
        this.oxygenConsumer = StartCoroutine(ConsumeOxygen());
    }

    /// <summary>
    /// Stops the Coroutine which consumes continuously oxgen if running.
    /// </summary>
    public void StopConsumeOxygen()
    {
        if (this.oxygenConsumer != null)
        {
            StopCoroutine(oxygenConsumer);
        }
    }

    /// <summary>
    /// Start a Coroutine which refills continuously oxgen.
    /// At the moment, only one of these coroutines at the time can be started.
    /// </summary>
    public void StartRefillOxygen()
    {
        this.oxygenRefill = StartCoroutine(RefillOxygen());
    }

    /// <summary>
    /// Stops the Coroutine which refills continuously oxgen if running.
    /// </summary>
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
            attributes.IncreaseOxygen(1);
            yield return new WaitForSeconds(OxygenRefillDelay);
        }
    }

    private IEnumerator ConsumeOxygen()
    {
        while (true)
        {
            attributes.DecreseOxygen(1);
            yield return new WaitForSeconds(OxygenConsumeDelay);
        }
    }
}