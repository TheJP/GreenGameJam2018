using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[RequireComponent(typeof(Animator))]
public class OxygenTower : Placeable, OxygenSink, EnergySink
{
    private const string AnimatorOnFlag = "On";

    [Tooltip("How much energy is used per game tick")]
    public Energy energyUsage = new Energy(10);

    // TODO: Remove (see ConsumeOxygen)
    private Oxygen oxygenUsage = new Oxygen(10);

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
        manager.RemoveSink((EnergySink)this);
        manager.RemoveSink((OxygenSink)this);
    }

    public void ConsumeEnergy(ResourceManager manager) =>
        HasEnergy = manager.TryConsume(energyUsage);

    public void ConsumeOxygen(ResourceManager manager)
    {
        // TODO: Check for attached players and only use oxygen to charge their tank (instead of consuming a constant amount each tick)
        bool on = HasEnergy && manager.TryConsume(oxygenUsage);

        // Update animation state
        if (animator.GetBool(AnimatorOnFlag) != on) { animator.SetBool(AnimatorOnFlag, on); }
    }
}
