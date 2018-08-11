using System.Collections;
using System.Collections.Generic;
using Resources;
using UnityEngine;

public class OxygenProducer
	: Placeable, OxygenSource
{
	[SerializeField]
	[Tooltip("The necessary energy for every unit of produced oxygen")]
	private Energy energyCostPerOxygenUnit;

	[SerializeField]
	[Tooltip("Maximum oxygen production per second")]
	private Oxygen oxygenPerSecond;

	private ResourceManager resourceManager;
	
	protected override void Start()
	{
		base.Start();
		
		resourceManager = FindObjectOfType<ResourceManager>();
		resourceManager.AddSource(this);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		
		resourceManager.RemoveSource(this);
	}

	public void ProduceOxygen(ResourceManager manager)
	{
		var oxygenProduction = oxygenPerSecond * Time.fixedDeltaTime;
		var energyNeeded = (float)oxygenProduction * energyCostPerOxygenUnit;

		if (manager.TryConsume(energyNeeded))
		{
			manager.Store(oxygenProduction);
		}
	}
}
