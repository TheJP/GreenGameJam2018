using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[RequireComponent(typeof(Animator))]
public class OxygenTower : Placeable, OxygenSink, EnergySink
{
    public const string AnimatorOnFlag = "On";

    [Tooltip("How much energy is used per game tick")]
    public Energy energyUsagePerSecond = new Energy(10);

    [Tooltip("How much oxygen is used per game tick")]
    public Oxygen oxygenUsagePerSecond = new Oxygen(5);

    [Tooltip("Area of effect that this tower provides with oxygen")]
    public float aoeRadius = 2f;

    private Animator animator;
    private ResourceManager manager;

    /// <summary>Internal state that determines if the machine has power and should consume oxygen.</summary>
    private bool HasEnergy { get; set; } = false;

    private void Awake() => animator = GetComponent<Animator>();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }

    public void ConsumeEnergy(ResourceManager manager) =>
        HasEnergy = manager.TryConsume(energyUsagePerSecond * Time.fixedDeltaTime);

    public void ConsumeOxygen(ResourceManager manager)
    {
        var on = HasEnergy && manager.TryConsume(oxygenUsagePerSecond * Time.fixedDeltaTime);
        if (animator.GetBool(AnimatorOnFlag) != on) { animator.SetBool(AnimatorOnFlag, on); }
    }
}
