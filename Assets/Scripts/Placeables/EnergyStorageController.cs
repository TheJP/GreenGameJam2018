using System;
using Resources;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EnergyStorage))]
public class EnergyStorageController : Placeable
{
	private const string AnimatorPercent = "Percent";
	
	private Animator animator;
	private EnergyStorage energyStorage;

	private ResourceManager resourceManager;
	
	protected override void Start ()
	{
		base.Start();
		
		animator = GetComponent<Animator>();
		energyStorage = GetComponent<EnergyStorage>();

		resourceManager = FindObjectOfType<ResourceManager>();
		resourceManager.AddStorage(energyStorage);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		
		resourceManager.RemoveStorage(energyStorage);
	}

	private void Update ()
	{
		var energyPercent = energyStorage.CurrentValue / energyStorage.Capacity;
		animator.SetFloat(AnimatorPercent, energyPercent);
	}
}
