using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
public class OxygenTower : Placeable, OxygenSink, EnergySink
{
    public const string AnimatorOnFlag = "On";

    [Tooltip("How much energy is used per game tick")]
    public Energy energyUsagePerSecond = new Energy(10);

    [Tooltip("How much oxygen is used per game tick")]
    public Oxygen minOxygenUsagePerSecond = new Oxygen(5);

    [Tooltip("How much oxygen is used per game tick")]
    public Oxygen oxygenRefillPerSecond = new Oxygen(10);

    [Tooltip("Area of effect that this tower provides with oxygen")]
    public float aoeRadius = 2f;

    private Animator animator;
    private ResourceManager manager;
    private LineRenderer lineRenderer;

    private readonly HashSet<Player> connectedPlayers = new HashSet<Player>();

    /// <summary>Internal state that determines if the machine has power and should consume oxygen.</summary>
    private bool HasEnergy { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        manager = FindObjectOfType<ResourceManager>();
        manager.AddSink((EnergySink)this);
        manager.AddSink((OxygenSink)this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        manager.RemoveSink((EnergySink)this);
        manager.RemoveSink((OxygenSink)this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null) { connectedPlayers.Add(player); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null) { connectedPlayers.Remove(player); }
    }

    private void Update()
    {
        var lines = connectedPlayers.SelectMany(p => new[] { transform.position, p.transform.position, transform.position }).ToArray();
        lineRenderer.positionCount = lines.Length;
        lineRenderer.SetPositions(lines);
    }

    public void ConsumeEnergy(ResourceManager manager) =>
        HasEnergy = manager.TryConsume(energyUsagePerSecond * Time.fixedDeltaTime);

    public void ConsumeOxygen(ResourceManager manager)
    {
        var on = false;
        if (HasEnergy)
        {
            // Can only provide oxygen that is stored and at maximum oxygen covering the refilling rate
            var oxygenAvailable = ResourceMath.Min(oxygenRefillPerSecond * Time.fixedDeltaTime, manager.OxygenAvailable);
            if (Mathf.Abs((float)oxygenAvailable) > Mathf.Epsilon)
            {
                // Calculate amount of oxygen that player would like to have
                var maxOxygenRequest = connectedPlayers.Aggregate(Oxygen.Zero, (a, b) => a + b.MaxReceiveOxygen());
                // Amount of oxygen that will be provided to the players
                var oxygenRequest = ResourceMath.Min(oxygenAvailable, maxOxygenRequest);
                // At least consume a minimal amount of oxygen
                // Without this, the tower wont work. Excess is lost.
                oxygenRequest = ResourceMath.Max(oxygenRequest, minOxygenUsagePerSecond * Time.fixedDeltaTime);

                if (manager.TryConsume(oxygenRequest))
                {
                    on = true;
                    ShareOxygenFairly(oxygenRequest);
                }
            }
        }

        if (animator.GetBool(AnimatorOnFlag) != on) { animator.SetBool(AnimatorOnFlag, on); }
    }

    /// <summary>Provide given amount of oxygen fairly among the connected players.</summary>
    /// <param name="oxygen">Oxygen to be provided.</param>
    private void ShareOxygenFairly(Oxygen oxygen)
    {
        // Start with the player that needs the least amount.
        // Give said player their fair share (or less if they'd be already overfilled).
        // Give the next player their fair share. (This might have increased if the previous player didn't use their full share).
        // ...
        var playerCount = connectedPlayers.Count;
        foreach (var player in connectedPlayers.OrderBy(p => p.MaxReceiveOxygen()))
        {
            var providing = ResourceMath.Min(player.MaxReceiveOxygen(), oxygen / playerCount);
            if (player.Receive(providing)) { oxygen -= providing; }
            --playerCount;
        }
    }
}
